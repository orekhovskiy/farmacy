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
        private FarmacyWebApiContext db;
        public MedicineController(FarmacyWebApiContext context)
        {
            db = context;
        }

        public IEnumerable<Medicine> GetAllMedicines() => db.Medicine;

        public IEnumerable<Medicine> GetFilteredMedicines([FromQuery] string[] producer, [FromQuery] string[] type, [FromQuery] string[] form, [FromQuery] string[] component, [FromQuery] int[] shelfTime, [FromQuery] bool[] available)
        {
            // TODFO
            return null;
        }

        public IEnumerable<Medicine> GetMedicinesByName([FromQuery] string name) => db.Medicine.Where(m => m.Name == name);

        public IEnumerable<Medicine> GetMedicinesByProducer([FromQuery] string producer) => db.Medicine.Where(m => m.Producer.Name == producer);

        public IEnumerable<string> GetAllMedicineNames() =>db.Medicine.Select(m => m.Name);

        public IEnumerable<string> GetAllMedicineProducers() => db.Producer.Select(p => p.Name);

        public IEnumerable<string> GetAllMedicineCategories() => db.Category.Select(c => c.Name);

        public IEnumerable<string> GetAllMedicineForms() => db.Form.Select(f => f.Name);

        public IEnumerable<string> GetAllMedicineComponents() => db.Component.Select(c => c.Name);

        public IEnumerable<int> GetAllMedicineShelfTimes() => db.Medicine.Select(m => m.ShelfTime);

        [HttpPost]
        public void NewMedicine([FromQuery] string name, [FromQuery] string producer, [FromQuery] string category, [FromQuery] string form, [FromQuery] string[] component, [FromQuery] int shelfTime, [FromQuery] int count)
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
            foreach (string c in component)
            {
                db.MedicineComposition.Add(new MedicineComposition
                {
                    Medicine = m,
                    Component = db.Component.Where(a => a.Name == c).First()
                }) ;
            }
            db.SaveChanges();
        }
        /*
        * GetAllMedicine
        * NewMedicine
        * AlterMedicine
        * SellMedicine
        */
    }
}