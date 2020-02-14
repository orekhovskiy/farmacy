using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmacyWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmacyWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly FarmacyWebApiContext db;
        public MedicineController(FarmacyWebApiContext context)
        {
            db = context;
        }

        public ICollection<MedicineComposition> GetMedicineComposition(int id)
        {
            return db.MedicineComposition.Where(mc => mc.MedicineId == id).Join
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
                ).ToList();
        }

        //Уродский метод #1
        //automapper
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

        // Уродский метод #2
        //https://localhost:44388/api/medicine/GetFilteredMedicines?producer=Bayer&category=%D0%90%D0%BD%D0%B0%D0%BB%D1%8C%D0%B3%D0%B5%D1%82%D0%B8%D0%BA%D0%B8&form=%D0%A2%D0%B0%D0%B1%D0%BB%D0%B5%D1%82%D0%BA%D0%B8&component=%D0%90%D1%86%D0%B5%D1%82%D0%B8%D0%BB%D1%81%D0%B0%D0%BB%D0%B8%D1%86%D0%B8%D0%BB%D0%BE%D0%B2%D0%B0%D1%8F%20%D0%BA%D0%B8%D1%81%D0%BB%D0%BE%D1%82%D0%B0&shelfTime=60&available=true
        public IEnumerable<Medicine> GetFilteredMedicines([FromQuery] string[] producer, [FromQuery] string[] category, 
            [FromQuery] string[] form, [FromQuery] string[] component, [FromQuery] int[] shelfTime, [FromQuery] bool[] available)
        {
            return GetAllMedicines().Where ( m =>   producer.Contains(m.Producer.Name) &&
                                                    category.Contains(m.Category.Name) &&
                                                    form.Contains(m.Form.Name) &&
                                                    component.Intersect(m.MedicineComposition.Select(mc => mc.Component.Name)).Any() &&
                                                    shelfTime.Contains(m.ShelfTime) &&
                                                    available.Contains(m.Count > 0 ? true : false));
        }

        public IEnumerable<Medicine> GetMedicinesByName([FromQuery] string name) => db.Medicine.Where(m => m.Name == name);

        public IEnumerable<Medicine> GetMedicinesByProducer([FromQuery] string producer) => db.Medicine.Where(m => m.Producer.Name == producer);

        public IEnumerable<string> GetAllMedicineNames() =>db.Medicine.Select(m => m.Name);

        public IEnumerable<string> GetAllMedicineProducers() => db.Producer.Select(p => p.Name);

        public IEnumerable<string> GetAllMedicineCategories() => db.Category.Select(c => c.Name);

        public IEnumerable<string> GetAllMedicineForms() => db.Form.Select(f => f.Name);

        public IEnumerable<string> GetAllMedicineComponents() => db.Component.Select(c => c.Name);

        public IEnumerable<string> GetMedicineComponents([FromQuery] int id) 
            => db.MedicineComposition.Where(mc => mc.MedicineId == id).Select(mc => mc.Component.Name);

        public IEnumerable<int> GetAllMedicineShelfTimes() => db.Medicine.Select(m => m.ShelfTime);

        
        public void NewMedicine([FromQuery] string name, [FromQuery] string producer, [FromQuery] string category, [FromQuery] string form, [FromQuery] string[] component, [FromQuery] int shelfTime, [FromQuery] int count)
        {
            //https://localhost:44388/api/medicine/NewMedicine?name=%D0%92%D0%BE%D0%B4%D0%B0%20%D0%B4%D0%B5%D1%81%D1%82%D0%B8%D0%BB%D0%B8%D1%80%D0%BE%D0%B2%D0%B0%D0%BD%D0%BD%D0%B0%D1%8F&producer=Bayer&category=%D0%94%D1%80%D1%83%D0%B3%D0%BE%D0%B5&form=%D0%96%D0%B8%D0%B4%D0%BA%D0%BE%D0%B5%20%D0%B2%D0%B5%D1%89%D0%B5%D1%81%D1%82%D0%B2%D0%BE&component=%D0%92%D0%BE%D0%B4%D0%B0&shelfTime=12&count=1      
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

        public void AlterMedicine([FromQuery] int id, [FromQuery] string name, [FromQuery] string producer, [FromQuery] string category, [FromQuery] string form, [FromQuery] string[] component, [FromQuery] int shelfTime, [FromQuery] int count)
        {
            //https://localhost:44388/api/medicine/AlterMedicine?id=3&name=%D0%92%D0%BE%D0%B4%D0%B0%20%D0%B4%D0%B5%D1%81%D1%82%D0%B8%D0%BB%D0%B8%D1%80%D0%BE%D0%B2%D0%B0%D0%BD%D0%BD%D0%B0%D1%8F&producer=Bayer&category=%D0%94%D1%80%D1%83%D0%B3%D0%BE%D0%B5&form=%D0%96%D0%B8%D0%B4%D0%BA%D0%BE%D0%B5%20%D0%B2%D0%B5%D1%89%D0%B5%D1%81%D1%82%D0%B2%D0%BE&component=%D0%92%D0%BE%D0%B4%D0%B0&shelfTime=12&count=3
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

        public void SellMedicine([FromQuery] int id, [FromQuery] int amount) => db.Medicine.Find(id).Count += amount;
    }
}