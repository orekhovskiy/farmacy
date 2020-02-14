using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmacyWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmacyWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private FarmacyWebApiContext db;
        public UserController(FarmacyWebApiContext context)
        {
            db = context;
            /*var u = new User
            {
                Login ="admin",
                Password = Hasher.GetHash("admin"),
                Firstname = "admin",
                Lastname = "admin",
                Position = 1
            };
            db.User.Add(u);
            db.SaveChanges();*/
        }

        public User GetUser([FromQuery] string login, [FromQuery] string password)
        {
            return db.User.Where(u => u.Login == login && Hasher.GetHash(password) == u.Password).FirstOrDefault();
        }

        public bool ValidateUser([FromQuery] string login, [FromQuery] string password) 
        {
            List<User> res = db.User.Where(u => u.Login == login && Hasher.GetHash(password) == u.Password).ToList();
            return res.Count > 0;
        }

        public int GetUserPosition([FromQuery] string login)
        {
           return db.User.Where(u => u.Login == login).FirstOrDefault().Position;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return db.User.ToList();
        }

        public void AddUser([FromForm] string login, [FromForm] string password, [FromForm] string firstname,
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