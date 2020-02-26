using Farmacy.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        private bool PostRequest(string requestString, HttpContent content)
        {
            return client.PostAsync(URL + requestString, content).IsCompleted;
        }

       /* public async Task<UserViewModel> GetUser(string login, string password)
    => await GetRequest<UserViewModel>($"GetUser?login={login}&password={password}");*/

        public bool AlterMedicine(MedicineViewModel mediciene)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetAllMedicineCategories()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetAllMedicineComponents()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetAllMedicineForms()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetAllMedicineProducers()
        {
            throw new NotImplementedException();
        }

        public Task<MedicineListViewModel> GetAllMedicinesPaged(int currentPage, int rowsOnPage)
        {
            throw new NotImplementedException();
        }

        public Task<ComponentSetViewModel> GetComponentSet(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MedicineListViewModel> GetFilteredMedicinesPaged(int currentPage, int rowsOnPage, string[] producer, string[] category, string[] form, string[] component, int[] shelfTime, bool[] available)
        {
            throw new NotImplementedException();
        }

        public Task<MedicineViewModel> GetMedicineById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetMedicineComponents(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MedicineListViewModel> GetMedicinesByKeyPaged(int currentPage, int rowsOnPage, string key)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<OptionSetViewModel>> GetOptionSet()
        {
            throw new NotImplementedException();
        }

        public bool NewMedicine(MedicineViewModel medicine)
        {
            throw new NotImplementedException();
        }

        public bool SellMedicine(MedicineViewModel medicine)
        {
            throw new NotImplementedException();
        }
    }
}
