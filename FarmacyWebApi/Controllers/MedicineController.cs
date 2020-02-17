using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmacyWebApi.Models;
using FarmacyWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmacyWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private IMedicineService _medicineService;

        public MedicineController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        //automapper
        
        private int GetPagesAmount(int rowsOnPage, int rowsAmount)
        {
            var r = (double)rowsAmount / rowsOnPage;
            return (int) Math.Ceiling(r);
        }

        private MedicineList GetPagedMedicines(IEnumerable<Medicine> medicines, int currentPage, int rowsOnPage)
        {
            int pagesAmount = GetPagesAmount(rowsOnPage, medicines.Count());
            if (currentPage > pagesAmount || currentPage < 1) return null;
            return new MedicineList
            {
                CurrentPage = currentPage,
                PagesAmount = pagesAmount,
                Medicines = medicines.Skip((currentPage - 1) * rowsOnPage).Take(rowsOnPage).ToList(),
            };
        }

        public MedicineList GetAllMedicinesPaged([FromQuery] int currentPage, [FromQuery] int rowsOnPage)
            => GetPagedMedicines(_medicineService.GetAllMedicines(), currentPage, rowsOnPage);

        public MedicineList GetFilteredMedicinesPaged([FromQuery] int currentPage, [FromQuery] int rowsOnPage, [FromQuery] string[] producer,
            [FromQuery] string[] category, [FromQuery] string[] form, [FromQuery] string[] component, [FromQuery] int[] shelfTime, [FromQuery] bool[] available)
            => GetPagedMedicines(_medicineService.GetFilteredMedicines(producer, category, form, component, shelfTime, available), currentPage, rowsOnPage);

        public  MedicineList GetMedicinesByKeyPaged([FromQuery] int currentPage, [FromQuery] int rowsOnPage ,[FromQuery] string key)
            => GetPagedMedicines(_medicineService.GetMedicinesByName(key).Concat(_medicineService.GetMedicinesByProducer(key)), currentPage, rowsOnPage);

        public IEnumerable<string> GetAllMedicineProducers() => _medicineService.GetAllMedicineProducers();
        
        public IEnumerable<string> GetAllMedicineCategories() => _medicineService.GetAllMedicineCategories();

        public IEnumerable<string> GetAllMedicineForms() => _medicineService.GetAllMedicineForms();

        public IEnumerable<string> GetAllMedicineComponents() => _medicineService.GetAllMedicineComponents();

        public IEnumerable<int> GetAllMedicineShelfTimes() => _medicineService.GetAllMedicineShelfTimes();

        public void NewMedicine([FromQuery] string name, [FromQuery] string producer, [FromQuery] string category, [FromQuery] string form, [FromQuery] string[] component, [FromQuery] int shelfTime, [FromQuery] int count)
             => _medicineService.NewMedicine(name, producer, category, form, component, shelfTime, count);

        public void AlterMedicine([FromQuery] int id, [FromQuery] string name, [FromQuery] string producer, [FromQuery] string category, [FromQuery] string form, [FromQuery] string[] component, [FromQuery] int shelfTime, [FromQuery] int count)
            => _medicineService.AlterMedicine(id, name, producer, category, form, component, shelfTime, count);

        public void SellMedicine([FromQuery] int id, [FromQuery] int amount) => _medicineService.SellMedicine(id, amount);
    }
}