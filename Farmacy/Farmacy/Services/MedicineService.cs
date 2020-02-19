using Farmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farmacy.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly FarmacyWebApiContext db;

        public MedicineService(FarmacyWebApiContext context)
        {
            db = context;
        }

        public void AlterMedicine(int id, string name, string producer, string category, string form, string[] component, int shelfTime, int count)
        {
            var m = db.Medicine.Find(id);
            m.Name = name;
            m.Producer = db.Producer.Where(p => p.Name == producer).First();
            m.Category = db.Category.Where(c => c.Name == category).First();
            m.Form = db.Form.Where(f => f.Name == form).First();
            m.ShelfTime = shelfTime;
            m.Count = count;
            db.SaveChanges();
            foreach (MedicineComposition mc in db.MedicineComposition.Where(mc => mc.MedicineId == id))
                db.Remove(mc);
            db.SaveChanges();
            if (component != null)
                foreach (string comp in component)
                    db.MedicineComposition.Add(new MedicineComposition
                    {
                        Component = db.Component.Where(c => c.Name == comp).First(),
                        Medicine = db.Medicine.Find(id)
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

        public void NewMedicine(string name, string producer, string category, string form, string[] component, int shelfTime, int count)
        {
            var m = new Medicine
            {
                Name = name,
                Producer = db.Producer.Where(p => p.Name == producer).First(),
                Category = db.Category.Where(c => c.Name == category).First(),
                Form = db.Form.Where(f => f.Name == form).First(),
                ShelfTime = shelfTime,
                Count = count
            };
            db.Medicine.Add(m);
            if (component != null)
                foreach (string c in component)
                    db.MedicineComposition.Add(new MedicineComposition
                    {
                        Medicine = m,
                        Component = db.Component.Where(a => a.Name == c).First()
                    });
            db.SaveChanges();
        }

        public void SellMedicine(int id, int amount)
        {
            var medicine = db.Medicine.Find(id);
            if (medicine.Count - amount >= 0)
            {
                medicine.Count -= amount;
                db.SaveChanges();
            }
        }

        public IEnumerable<string> GetMedicineComponents(int id) => db.MedicineComposition.Where(mc => mc.MedicineId == id).Select(mc => mc.Component.Name);

        public Medicine GetMedicineById(int id) => GetAllMedicines().Where(m => m.Id == id).First();
    }
}
