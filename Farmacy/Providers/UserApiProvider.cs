﻿using Microsoft.Extensions.Configuration;
using Farmacy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;

namespace Farmacy.Providers
{
    public class UserApiProvider : IUserApiProvider
    {
        private readonly string URL;
        static HttpClient client = new HttpClient();
        public UserApiProvider(IConfiguration configuration)
        {
            URL = configuration.GetValue<string>("UserApiUrl");
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

        public async Task<UserViewModel> GetUser(string login, string password) 
            => await GetRequest<UserViewModel>($"GetUser?login={login}&password={password}");

        public async Task<bool> ValidateUser(string login, string password)
            => await GetRequest<bool>($"ValidateUser?login={login}&password={password}");

        public async Task<int> GetUserPositionId(int id)
            => await GetRequest<int>($"GetUserPostionId?id={id}");

        public async Task<string> GetUserPosition(int id)
            => await GetRequest<string>($"GetUserPosition?id={id}");

        public async Task<IEnumerable<UserViewModel>> GetAllUsers()
            => await GetRequest<IEnumerable<UserViewModel>>($"");

        public async Task<bool> AddUser(UserViewModel user)
        {
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            return await PostRequest("AddUser", content);
        }
    }
}
