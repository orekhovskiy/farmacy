using Farmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farmacy.Services
{
    public interface IUserService
    {
        public User GetUser(string login, string password);
        public int GetUserPositionId(int id);
        public string GetUserPosition(int id);
        public IEnumerable<User> GetAllUsers();
        public void AddUser(string login, string password, string firstname, string lastname, int position);
    }
}
