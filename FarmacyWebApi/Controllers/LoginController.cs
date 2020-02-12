using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmacyWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarmacyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private FarmacyWebApiContext db;
        public LoginController(FarmacyWebApiContext context)
        {
            db = context;
        }

        [Route("GetUsers")]
        public IEnumerable<User> GetEmployees()
        {
            return db.User.ToList();
        }

        [Route("GetUser")]
        public User GetFirstEmployee()
        {
            return db.User.First();
        }

        [HttpPost]
        [ActionName("AddUser")]
        public void Post([FromForm] string login, [FromForm] string password, [FromForm] string firstname,
            [FromForm] string lastname, [FromForm] int position)
        {
            var u = new User
            {
                Login = login,
                Password = Hasher.GetHash(password),
                Firstname = firstname,
                Lastname = lastname,
                Position = position
            };
            db.User.Add(u);
            db.SaveChanges();
        }
    }
}