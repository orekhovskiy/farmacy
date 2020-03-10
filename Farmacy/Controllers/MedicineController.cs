using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Farmacy.Providers;
using Farmacy.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Farmacy.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private IMedicineApiProvider _medicineApiProvider;
        private readonly ILogger<MedicineController> _logger;

        public MedicineController(IMedicineApiProvider medicineApiProvider, ILogger<MedicineController> logger)
        {
            _medicineApiProvider = medicineApiProvider;
            _logger = logger;
            _logger.LogDebug("NLog injected into MedicineController");
        }

        private string GetLogin(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(token.Substring(7)) as JwtSecurityToken;
            return tokenS.Claims.Where(claim => claim.Type == ClaimsIdentity.DefaultNameClaimType).First().Value;
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

        [Authorize(Roles ="admin, manager"), HttpGet]
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
            string login = GetLogin(Request.Headers["Authorization"].First());
            bool result = default;
            try
            {
                result = await _medicineApiProvider.NewMedicine(medicine, login);
                _logger.LogInformation($"New medicine with \"{medicine.Name}\" name has been added by \"{login}\" user"); 
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Attempt of adding new medicine with \"{medicine.Name}\" name failed with following exception: {e}");
            }
            return result;
        }


        [Authorize(Roles = "admin, manager"), HttpGet]
        [ActionName("AlterMedicine")]
        public async Task<bool> AlterMedicine([FromQuery] int id, [FromQuery] string name, [FromQuery] string producer, 
            [FromQuery] string category, [FromQuery] string form, [FromQuery] string[] component, [FromQuery] int shelfTime, [FromQuery] int count)
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
                Count = count
            };
            string login = GetLogin(Request.Headers["Authorization"].First());
            bool result = default;
            try
            {
                result = await _medicineApiProvider.AlterMedicine(medicine, login);
                _logger.LogInformation($"Medicine with \"{medicine.Name}\" name has been altered by \"{login}\" user");
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Attempt of altering medicine with \"{medicine.Name}\" name has been failed with following exception: {e}");
            }
            return result;
        }

        [Authorize(Roles = "seller"), HttpGet]
        [ActionName("SellMedicine")]
        public async Task<bool> SellMedicine([FromQuery] int id, [FromQuery] int count)
        {
            string login = GetLogin(Request.Headers["Authorization"].First());
            bool result = default;
            try
            {
                result = await _medicineApiProvider.SellMedicine(id, count, login);
                _logger.LogInformation($"Medicine with \"{id}\" id has been selled in amount of \"{count}\" by \"{login}\" user.");
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Attempt of selling medicine with \"{id}\" id failed with following exception: {e}");
            }
            return result;
        }

        [Authorize(Roles = "admin, manager"), HttpGet]
        [ActionName("GetMedicineById")]
        public async Task<MedicineViewModel> GetMedicineById([FromQuery] int id) => await _medicineApiProvider.GetMedicineById(id);

        [Authorize(Roles = "admin, manager"), HttpGet]
        [ActionName("GetMedicineComponents")]
        public async Task<IEnumerable<string>> GetMedicineComponents([FromQuery] int id) => await _medicineApiProvider.GetMedicineComponents(id);

        [Authorize(Roles = "admin, manager"), HttpGet]
        [ActionName("GetAllmedicineNames")]
        public async Task<IEnumerable<string>> GetAllMedicineNames() => await _medicineApiProvider.GetAllMedicineNames();

        [Authorize(Roles = "admin, manager"), HttpGet]
        [ActionName("GetAllMedicineProducers")]
        public async Task<IEnumerable<string>> GetAllMedicineProducers() => await _medicineApiProvider.GetAllMedicineProducers();

        [Authorize(Roles = "admin, manager"), HttpGet]
        [ActionName("GetAllMedicineCategories")]
        public async Task<IEnumerable<string>> GetAllMedicineCategories() => await _medicineApiProvider.GetAllMedicineCategories();

        [Authorize(Roles = "admin, manager"), HttpGet]
        [ActionName("GetAllMedicineForms")]
        public async Task<IEnumerable<string>> GetAllMedicineForms() => await _medicineApiProvider.GetAllMedicineForms();

        [Authorize(Roles = "admin, manager"), HttpGet]
        [ActionName("GetAllMedicineComponents")]
        public async Task<IEnumerable<string>> GetAllMedicineComponents() => await _medicineApiProvider.GetAllMedicineComponents();

        [Authorize(Roles = "admin, manager"), HttpGet]
        [ActionName("GetComponentSet")]
        public async Task<ComponentSetViewModel> GetComponentSet([FromQuery] int id) => await _medicineApiProvider.GetComponentSet(id);
    }
}