using MedicineApi.Models;
using MedicineApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicineApi.Services
{
    public interface IMedicineService
    {
        public IEnumerable<Medicine> GetAllMedicines();
        public IEnumerable<Medicine> GetFilteredMedicines(string[] producer, string[] category, string[] form, string[] component, int[] shelfTime, bool[] available);
        public IEnumerable<Medicine> GetMedicinesByName(string name);
        public IEnumerable<Medicine> GetMedicinesByProducer(string producer);
        public IEnumerable<string> GetMedicineComponents(int id);
        public IEnumerable<string> GetAllMedicineProducers();
        public IEnumerable<string> GetAllMedicineCategories();
        public IEnumerable<string> GetAllMedicineForms();
        public IEnumerable<string> GetAllMedicineComponents();
        public IEnumerable<int> GetAllMedicineShelfTimes();
        public void NewMedicine(MedicineViewModel medicine);
        public void AlterMedicine(MedicineViewModel medicine);
        public void SellMedicine(string login, MedicineViewModel medicine);
        public Medicine GetMedicineById(int id);
    }
}
