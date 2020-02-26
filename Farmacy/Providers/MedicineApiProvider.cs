using Farmacy.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Farmacy.Providers
{
    public class MedicineApiProvider : IMedicineApiProvider
    {
        private readonly string URL;
        static HttpClient client = new HttpClient();
        public MedicineApiProvider(IConfiguration configuration)
        {
            URL = configuration.GetValue<string>("MedicineApiUrl");
        }

        private async Task<T> GetRequest<T>(string requestString)
        {
            HttpResponseMessage responce = await client.GetAsync(URL + requestString);
            if (responce.IsSuccessStatusCode)
            {
                return await responce.Content.ReadAsAsync<T>();
            }
            return default;
        }

        private async Task<bool> PostRequest(string requestString, HttpContent content)
        {
            var responce = await client.PostAsync(URL + requestString, content);
            return responce.IsSuccessStatusCode;
        }

        public Task<bool> AlterMedicine(MedicineViewModel medicine)
        {
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(medicine), Encoding.UTF8, "application/json");
            return PostRequest("AlterMedicine", content);
        }

        public async Task<IEnumerable<string>> GetAllMedicineCategories() => await GetRequest<IEnumerable<string>>($"GetAllMedicineCategories");

        public async Task<IEnumerable<string>> GetAllMedicineComponents() => await GetRequest<IEnumerable<string>>($"GetAllMedicineComponents");

        public async Task<IEnumerable<string>> GetAllMedicineForms() => await GetRequest<IEnumerable<string>>($"GetAllMedicineForms");

        public async Task<IEnumerable<string>> GetAllMedicineProducers() => await GetRequest<IEnumerable<string>>($"GetAllMedicineProducers");

        public async Task<MedicineListViewModel> GetAllMedicinesPaged(int currentPage, int rowsOnPage) 
            => await GetRequest<MedicineListViewModel>($"GetAllMedicinesPaged?currentpage={currentPage}&rowsOnPage={rowsOnPage}");

        public async Task<ComponentSetViewModel> GetComponentSet(int id) => await GetRequest<ComponentSetViewModel>($"GetComponentSet?id={id}");

        public async Task<MedicineListViewModel> GetFilteredMedicinesPaged(int currentPage, int rowsOnPage, string[] producer, string[] category, string[] form, string[] component, int[] shelfTime, string[] available)
        {
            string query = $"GetFilteredMedicinesPaged?currentPage={currentPage}&rowsOnPage={rowsOnPage}";
            foreach (var element in producer) query += $"&producer={element}";
            foreach (var element in category) query += $"&category={element}";
            foreach (var element in form) query += $"&form={element}";
            foreach (var element in component) query += $"&component={element}";
            foreach (var element in shelfTime) query += $"&shelfTime={element}";
            foreach (var element in available) 
            {
                if (element == "Да")
                {
                    query += $"&available={true}";
                } else
                {
                    query += $"&available={false}";
                }
            }
            return await GetRequest<MedicineListViewModel>(query);
        }

        public async Task<MedicineViewModel> GetMedicineById(int id) => await GetRequest<MedicineViewModel>($"GetMedicineById?id={id}");

        public async Task<IEnumerable<string>> GetMedicineComponents(int id) => await GetRequest<IEnumerable<string>>($"GetMedicineComponents?id={id}");

        public async Task<MedicineListViewModel> GetMedicinesByKeyPaged(int currentPage, int rowsOnPage, string key)
            => await GetRequest<MedicineListViewModel>($"GetMedicinesByKeyPaged?currentPage={currentPage}&rowsOnPage={rowsOnPage}&key={key}");

        public async Task<ICollection<OptionSetViewModel>> GetOptionSet()
            => await GetRequest<ICollection<OptionSetViewModel>>($"GetOptionSet");

        public Task<bool> NewMedicine(MedicineViewModel medicine)
        {
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(medicine), Encoding.UTF8, "application/json");
            return PostRequest("NewMedicine", content);
        }
    }
}
