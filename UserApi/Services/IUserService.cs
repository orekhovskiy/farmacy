using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApi.Models;
using UserApi.ViewModels;

namespace UserApi.Services
{
    public interface IUserService
    {
        public User GetUser(string login, string password);
        public int GetUserPositionId(int id);
        public string GetUserPosition(int id);
        public IEnumerable<User> GetAllUsers();
        public void AddUser(UserViewModel user);
    }
}
