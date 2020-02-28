using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicineApi.Models;
using MedicineApi.ViewModels;
using MedicineApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicineApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _medicineService;
        private readonly IMapper _mapper;

        public MedicineController(IMedicineService medicineService, IMapper mapper)
        {
            _medicineService = medicineService;
            _mapper = mapper;
        }

        private int GetPagesAmount(int rowsOnPage, int rowsAmount)
        {
            var r = (double)rowsAmount / rowsOnPage;
            return (int)Math.Ceiling(r);
        }

        private MedicineListViewModel GetPagedMedicines(IEnumerable<Medicine> medicines, int currentPage, int rowsOnPage)
        {
            int pagesAmount = GetPagesAmount(rowsOnPage, medicines.Count());
            if (currentPage > pagesAmount || currentPage < 1) return new MedicineListViewModel { };
            return new MedicineListViewModel
            {
                CurrentPage = currentPage,
                PagesAmount = pagesAmount,
                Medicines = medicines.Skip((currentPage - 1) * rowsOnPage).Take(rowsOnPage).Select(m => _mapper.Map<MedicineViewModel>(m)).ToList(),
            };
        }

        [HttpGet]
        [ActionName("GetAllMedicinesPaged")]
        public MedicineListViewModel GetAllMedicinesPaged([FromQuery] int currentPage, [FromQuery] int rowsOnPage)
            => GetPagedMedicines(_medicineService.GetAllMedicines(), currentPage, rowsOnPage);

        [HttpGet]
        [ActionName("GetFilteredMedicinesPaged")]
        public MedicineListViewModel GetFilteredMedicinesPaged([FromQuery] int currentPage, [FromQuery] int rowsOnPage, [FromQuery] string[] producer,
            [FromQuery] string[] category, [FromQuery] string[] form, [FromQuery] string[] component, [FromQuery] int[] shelfTime, [FromQuery] bool[] available)
            => GetPagedMedicines(_medicineService.GetFilteredMedicines(producer, category, form, component, shelfTime, available), currentPage, rowsOnPage);

        [HttpGet]
        [ActionName("GetMedicinesByKeyPaged")]
        public MedicineListViewModel GetMedicinesByKeyPaged([FromQuery] int currentPage, [FromQuery] int rowsOnPage, [FromQuery] string key)
            => GetPagedMedicines(_medicineService.GetMedicinesByName(key).Concat(_medicineService.GetMedicinesByProducer(key)), currentPage, rowsOnPage);

        [HttpGet]
        [ActionName("GetOptionSet")]
        public ICollection<OptionSetViewModel> GetOptionSet()
        {
            var producer = new OptionSetViewModel { Key = "producer", Name = "Производитель", Options = _medicineService.GetAllMedicineProducers().ToList() };
            var category = new OptionSetViewModel { Key = "category", Name = "Категория", Options = _medicineService.GetAllMedicineCategories().ToList() };
            var form = new OptionSetViewModel { Key = "form", Name = "Форма", Options = _medicineService.GetAllMedicineForms().ToList() };
            var composition = new OptionSetViewModel { Key = "component", Name = "Состав", Options = _medicineService.GetAllMedicineComponents().ToList() };
            var shelfTime = new OptionSetViewModel { Key = "shelfTime", Name = "Срок годности", Options = _medicineService.GetAllMedicineShelfTimes().OrderBy(s => s).Select(s => s.ToString()).Distinct().ToList() };
            var result = new List<OptionSetViewModel> { producer, category, form, composition, shelfTime };
            return result;
        }

        [HttpPost]
        [ActionName("NewMedicine")]
        public void NewMedicine([FromBody] MedicineViewModel medicine)
             => _medicineService.NewMedicine(medicine);

        [HttpPost]
        [ActionName("AlterMedicine")]
        public void AlterMedicine([FromBody]  MedicineViewModel medicine)
            => _medicineService.AlterMedicine(medicine);

        [HttpPost]
        [ActionName("SellMedicine")]
        public void SellMedicine([FromBody] MedicineViewModel medicine) => _medicineService.SellMedicine(medicine);

        [HttpGet]
        [ActionName("GetMedicineById")]
        public MedicineViewModel GetMedicineById([FromQuery] int id) => _mapper.Map<MedicineViewModel>(_medicineService.GetMedicineById(id));

        [HttpGet]
        [ActionName("GetMedicineComponents")]
        public IEnumerable<string> GetMedicineComponents([FromQuery] int id) => _medicineService.GetMedicineComponents(id);

        [HttpGet]
        [ActionName("GetAllMedicineProducers")]
        public IEnumerable<string> GetAllMedicineProducers() => _medicineService.GetAllMedicineProducers();

        [HttpGet]
        [ActionName("GetAllMedicineCategories")]
        public IEnumerable<string> GetAllMedicineCategories() => _medicineService.GetAllMedicineCategories();

        [HttpGet]
        [ActionName("GetAllMedicineForms")]
        public IEnumerable<string> GetAllMedicineForms() => _medicineService.GetAllMedicineForms();

        [HttpGet]
        [ActionName("GetAllMedicineComponents")]
        public IEnumerable<string> GetAllMedicineComponents() => _medicineService.GetAllMedicineComponents();

        [HttpGet]
        [ActionName("GetComponentSet")]
        public ComponentSetViewModel GetComponentSet([FromQuery] int id)
        {
            ICollection<string> allComponents = _medicineService.GetAllMedicineComponents().ToList();
            ICollection<string> currentComponents = _medicineService.GetMedicineComponents(id).ToList();
            ICollection<string> availableComponents = new List<string> { };
            foreach (string component in allComponents)
            {
                if (!currentComponents.Contains(component)) availableComponents.Add(component);
            }
            return new ComponentSetViewModel
            {
                AvailableComponents = availableComponents,
                CurrentComponents = currentComponents
            };
        }
    }
}