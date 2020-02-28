using Farmacy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farmacy.Providers
{
    public interface IMedicineApiProvider
    {
        public Task<MedicineListViewModel> GetAllMedicinesPaged(int currentPage, int rowsOnPage);
        public Task<MedicineListViewModel> GetFilteredMedicinesPaged(int currentPage, int rowsOnPage, string[] producer, string[] category, string[] form, string[] component, int[] shelfTime, string[] available);
        public Task<MedicineListViewModel> GetMedicinesByKeyPaged(int currentPage, int rowsOnPage, string key);
        public Task<ICollection<OptionSetViewModel>> GetOptionSet();
        public Task<MedicineViewModel> GetMedicineById(int id);
        public Task<IEnumerable<string>> GetMedicineComponents(int id);
        public Task<IEnumerable<string>> GetAllMedicineProducers();
        public Task<IEnumerable<string>> GetAllMedicineCategories();
        public Task<IEnumerable<string>> GetAllMedicineForms();
        public Task<IEnumerable<string>> GetAllMedicineComponents();
        public Task<ComponentSetViewModel> GetComponentSet(int id);
        public Task<bool> NewMedicine(MedicineViewModel medicine);
        public Task<bool> AlterMedicine(MedicineViewModel medicine);
        public Task<bool> SellMedicine(int id, int count);
    }
}
