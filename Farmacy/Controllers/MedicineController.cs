using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Farmacy.Providers;
using Farmacy.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Farmacy.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private IMedicineApiProvider _medicineApiProvider;

        public MedicineController(IMedicineApiProvider medicineApiProvider)
        {
            _medicineApiProvider = medicineApiProvider;
        }

        [Authorize, HttpGet]
        [ActionName("GetAllMedicinesPaged")]
        public async Task<MedicineListViewModel> GetAllMedicinesPaged([FromQuery] int currentPage, [FromQuery] int rowsOnPage)
            => await _medicineApiProvider.GetAllMedicinesPaged(currentPage, rowsOnPage);

        [Authorize, HttpGet]
        [ActionName("GetFilteredMedicinesPaged")]
        public async Task<MedicineListViewModel> GetFilteredMedicinesPaged([FromQuery] int currentPage, [FromQuery] int rowsOnPage, [FromQuery] string[] producer,
            [FromQuery] string[] category, [FromQuery] string[] form, [FromQuery] string[] component, [FromQuery] int[] shelfTime, [FromQuery] string[] available)
            => await _medicineApiProvider.GetFilteredMedicinesPaged(currentPage, rowsOnPage,producer, category, form, component, shelfTime, available);

        [Authorize, HttpGet]
        [ActionName("GetMedicinesByKeyPaged")]
        public async Task<MedicineListViewModel> GetMedicinesByKeyPaged([FromQuery] int currentPage, [FromQuery] int rowsOnPage, [FromQuery] string key)
            => await _medicineApiProvider.GetMedicinesByKeyPaged(currentPage, rowsOnPage, key);

        [Authorize, HttpGet]
        [ActionName("GetOptionSet")]
        public async Task<ICollection<OptionSetViewModel>> GetOptionSet() => await _medicineApiProvider.GetOptionSet();

        [Authorize, HttpGet]
        [ActionName("NewMedicine")]
        public async Task<bool> NewMedicine([FromQuery] string name, [FromQuery] string producer, [FromQuery] string category, [FromQuery] string form, [FromQuery] string[] component, [FromQuery] int shelfTime, [FromQuery] int count)
        {
            MedicineViewModel medicine = new MedicineViewModel { 
                Name = name,
                Producer = producer,
                Category = category,
                Form = form,
                Components = component,
                ShelfTime = shelfTime,
                Count = count,
            };
             return await _medicineApiProvider.NewMedicine(medicine);
        }


        [Authorize, HttpGet]
        [ActionName("AlterMedicine")]
        public async Task<bool> AlterMedicine([FromQuery] int id, [FromQuery] string name, [FromQuery] string producer, [FromQuery] string category, [FromQuery] string form, [FromQuery] string[] component, [FromQuery] int shelfTime, [FromQuery] int count)
        {
            MedicineViewModel medicine = new MedicineViewModel
            {
                Id = id,
                Name = name,
                Producer = producer,
                Category = category,
                Form = form,
                Components = component,
                ShelfTime = shelfTime,
                Count = count,
            };
            return await _medicineApiProvider.AlterMedicine(medicine);
        }

        [Authorize, HttpGet]
        [ActionName("GetMedicineById")]
        public async Task<MedicineViewModel> GetMedicineById([FromQuery] int id) => await _medicineApiProvider.GetMedicineById(id);

        [Authorize, HttpGet]
        [ActionName("GetMedicineComponents")]
        public async Task<IEnumerable<string>> GetMedicineComponents([FromQuery] int id) => await _medicineApiProvider.GetMedicineComponents(id);

        [Authorize, HttpGet]
        [ActionName("GetAllMedicineProducers")]
        public async Task<IEnumerable<string>> GetAllMedicineProducers() => await _medicineApiProvider.GetAllMedicineProducers();

        [Authorize, HttpGet]
        [ActionName("GetAllMedicineCategories")]
        public async Task<IEnumerable<string>> GetAllMedicineCategories() => await _medicineApiProvider.GetAllMedicineCategories();

        [Authorize, HttpGet]
        [ActionName("GetAllMedicineForms")]
        public async Task<IEnumerable<string>> GetAllMedicineForms() => await _medicineApiProvider.GetAllMedicineForms();

        [Authorize, HttpGet]
        [ActionName("GetAllMedicineComponents")]
        public async Task<IEnumerable<string>> GetAllMedicineComponents() => await _medicineApiProvider.GetAllMedicineComponents();

        [Authorize, HttpGet]
        [ActionName("GetComponentSet")]
        public async Task<ComponentSetViewModel> GetComponentSet([FromQuery] int id) => await _medicineApiProvider.GetComponentSet(id);
    }
}