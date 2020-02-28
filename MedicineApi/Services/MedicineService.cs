using MedicineApi.Models;
using MedicineApi.Models.Context;
using MedicineApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicineApi.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly MedicineApiContext db;

        public MedicineService(MedicineApiContext context)
        {
            db = context;
        }

        public void AlterMedicine(string login, MedicineViewModel medicine)
        {
            var m = db.Medicine.Find(medicine.Id);
            var prevCount = m.Count;
            m.Name = medicine.Name;
            m.Producer = db.Producer.Where(p => p.Name == medicine.Producer).First();
            m.Category = db.Category.Where(c => c.Name == medicine.Category).First();
            m.Form = db.Form.Where(f => f.Name == medicine.Form).First();
            m.ShelfTime = medicine.ShelfTime;
            m.Count = medicine.Count;
            db.SaveChanges();
            foreach (MedicineComposition mc in db.MedicineComposition.Where(mc => mc.MedicineId == medicine.Id))
                db.Remove(mc);
            db.SaveChanges();
            if (medicine.Components != null)
                foreach (string comp in medicine.Components)
                    db.MedicineComposition.Add(new MedicineComposition
                    {
                        Component = db.Component.Where(c => c.Name == comp).First(),
                        Medicine = db.Medicine.Find(medicine.Id)
                    });
            db.Purchase.Add(new Purchase
            {
                UserId = db.User.Where(u => u.Login == login).FirstOrDefault().Id,
                MedicineId = medicine.Id,
                Operation = medicine.Count - prevCount,
                PurchaseDate = DateTime.UtcNow
            });
            db.SaveChanges();
        }

        public IEnumerable<Medicine> GetMedicinesByName(string name) => GetAllMedicines().Where(m => m.Name == name);

        public IEnumerable<string> GetAllMedicineCategories() => db.Category.Select(c => c.Name);

        public IEnumerable<string> GetAllMedicineComponents() => db.Component.Select(c => c.Name);

        public IEnumerable<string> GetAllMedicineForms() => db.Form.Select(f => f.Name);

        public IEnumerable<string> GetAllMedicineProducers() => db.Producer.Select(p => p.Name);

        public IEnumerable<Medicine> GetAllMedicines()
        {
            var result = from medicine in db.Medicine
                         join category in db.Category on medicine.CategoryId equals category.Id
                         join form in db.Form on medicine.FormId equals form.Id
                         join producer in db.Producer on medicine.ProducerId equals producer.Id
                         select new Medicine
                         {
                             Id = medicine.Id,
                             Name = medicine.Name,
                             ProducerId = medicine.ProducerId,
                             CategoryId = medicine.CategoryId,
                             FormId = medicine.FormId,
                             ShelfTime = medicine.ShelfTime,
                             Count = medicine.Count,
                             Category = category,
                             Form = form,
                             Producer = producer,
                             MedicineComposition = db.MedicineComposition.Where(mc => mc.MedicineId == medicine.Id).Join
                            (
                               db.Component,
                               mc => mc.ComponentId,
                               c => c.Id,
                               (mc, c) => new MedicineComposition
                               {
                                   MedicineId = mc.MedicineId,
                                   ComponentId = mc.ComponentId,
                                   Component = c
                               }
                            ).ToList()
                         };
            return result;
        }

        public IEnumerable<int> GetAllMedicineShelfTimes() => db.Medicine.Select(m => m.ShelfTime);

        public IEnumerable<Medicine> GetFilteredMedicines(string[] producer, string[] category, string[] form, string[] component, int[] shelfTime, bool[] available)
        {
            return GetAllMedicines().Where(m => producer.Contains(m.Producer.Name) &&
                                                category.Contains(m.Category.Name) &&
                                                form.Contains(m.Form.Name) &&
                                                component.Intersect(m.MedicineComposition.Select(mc => mc.Component.Name)).Any() &&
                                                shelfTime.Contains(m.ShelfTime) &&
                                                available.Contains(m.Count > 0 ? true : false));
        }

        public IEnumerable<Medicine> GetMedicinesByProducer(string producer) => GetAllMedicines().Where(m => m.Producer.Name == producer);

        public void NewMedicine(string login, MedicineViewModel medicine)
        {
            var m = new Medicine
            {
                Name = medicine.Name,
                Producer = db.Producer.Where(p => p.Name == medicine.Producer).First(),
                Category = db.Category.Where(c => c.Name == medicine.Category).First(),
                Form = db.Form.Where(f => f.Name == medicine.Form).First(),
                ShelfTime = medicine.ShelfTime,
                Count = medicine.Count
            };
            db.Medicine.Add(m);
            if (medicine.Components != null)
                foreach (string c in medicine.Components)
                    db.MedicineComposition.Add(new MedicineComposition
                    {
                        Medicine = m,
                        Component = db.Component.Where(a => a.Name == c).First()
                    });
            db.SaveChanges();
            db.Purchase.Add(new Purchase
            {
                UserId = db.User.Where(u => u.Login == login).FirstOrDefault().Id,
                MedicineId = db.Medicine.Where(m => m.Name == medicine.Name).FirstOrDefault().Id,
                Operation = medicine.Count,
                PurchaseDate = DateTime.UtcNow
            });
            db.SaveChanges();
        }

        public void SellMedicine(string login, MedicineViewModel medicine)
        {
            var m = db.Medicine.Find(medicine.Id);
            if (m.Count - medicine.Count >= 0)
            {
                db.Purchase.Add(new Purchase
                {
                    UserId = db.User.Where(u => u.Login == login).FirstOrDefault().Id,
                    MedicineId = medicine.Id,
                    Operation = -medicine.Count,
                    PurchaseDate = DateTime.UtcNow
                });

                m.Count -= medicine.Count;
            
                db.SaveChanges();
            }
        }

        public IEnumerable<string> GetMedicineComponents(int id) => db.MedicineComposition.Where(mc => mc.MedicineId == id).Select(mc => mc.Component.Name);

        public Medicine GetMedicineById(int id) => GetAllMedicines().Where(m => m.Id == id).First();

        public IEnumerable<string> GetAllMedicineNames() => db.Medicine.Select(m => m.Name);
    }
}
