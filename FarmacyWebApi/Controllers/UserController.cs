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
    [Route("api/[controller]")]
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

        [Route("GetUser")]
        [HttpGet]
        public User GetUser([FromQuery] string login, [FromQuery] string password)
        {
            return db.User.Where(u => u.Login == login && Hasher.GetHash(password) == u.Password).FirstOrDefault();
        }

        [Route("ValidateUser")]
        [HttpGet]
        public bool ValidateUSer([FromQuery] string login, [FromQuery] string password) 
        {
            List<User> res = db.User.Where(u => u.Login == login && Hasher.GetHash(password) == u.Password).ToList();
            return res.Count > 0;
        }

        [Route("GetUserPosition")]
        [HttpGet]
        public int GetUserPosition([FromQuery] string login)
        {
           return db.User.Where(u => u.Login == login).FirstOrDefault().Position;
        }

                #region
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
                #endregion
    }
}