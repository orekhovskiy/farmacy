using Farmacy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farmacy.Providers
{
    public interface IUserApiProvider
    {
        public Task<UserViewModel> GetUser(string login, string password);
        public Task<bool> ValidateUser(string login, string password);
        public Task<int> GetUserPositionId(int id);
        public Task<string> GetUserPosition(int id);
        public Task<IEnumerable<UserViewModel>> GetAllUsers();
        public bool AddUser(UserViewModel user);
    }
}
