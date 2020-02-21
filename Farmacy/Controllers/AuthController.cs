using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Farmacy.Commons;
using Farmacy.Providers;
using Farmacy.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Farmacy.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserApiProvider _userApiProvider;
        public AuthController(IUserApiProvider userApiProvider)
        {
            _userApiProvider = userApiProvider;
            // New user initialization in a new DB
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

        [HttpGet]
        [ActionName("GetUser")]
        public Task<UserViewModel> GetUser([FromQuery] string login, [FromQuery] string password) => _userApiProvider.GetUser(login, password);

        /*[HttpGet]
        [ActionName("ValidateUser")]
        public bool ValidateUser([FromQuery] string login, [FromQuery] string password) 
        {
            return (_userService.GetUser(login, password) != null);
        }

        [HttpGet]
        [ActionName("GetUserPosition")]
        public string GetUserPosition([FromQuery] int id) => _userService.GetUserPosition(id);

        [HttpGet]
        [ActionName("GetUserPositionId")]
        public int GetUserPositionId([FromQuery] int id) => _userService.GetUserPositionId(id);

        [HttpGet]
        [ActionName("GetAllUsers")]
        public IEnumerable<User> GetAllUsers() => _userService.GetAllUsers();

        [HttpGet]
        [ActionName("AddUser")]
        public void AddUser([FromForm] string login, [FromForm] string password, [FromForm] string firstname,
            [FromForm] string lastname, [FromForm] int position)
            => _userService.AddUser(login, password, firstname, lastname, position);*/
    }
}