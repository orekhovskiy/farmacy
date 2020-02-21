using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserApi.Models;
using UserApi.Services;
using UserApi.ViewModels;

namespace UserApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
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
        public UserViewModel GetUser([FromQuery] string login, [FromQuery] string password) => _mapper.Map<UserViewModel>(_userService.GetUser(login, password));

        [HttpGet]
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
        public IEnumerable<UserViewModel> GetAllUsers() 
            => _userService.GetAllUsers().Select(element => _mapper.Map<UserViewModel>(element));

        [HttpGet]
        [ActionName("AddUser")]
        public void AddUser([FromForm] string login, [FromForm] string password, [FromForm] string firstname,
            [FromForm] string lastname, [FromForm] int position)
            => _userService.AddUser(login, password, firstname, lastname, position);
    }
}