using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserApi.Models;
using UserApi.Services;
using UserApi.ViewModels;

namespace UserApi.Controllers
{
    /// <summary>
    /// UserApi main controller
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// UserController constructor
        /// </summary>
        public UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
            _logger.LogDebug("NLog injected into UserController");
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

        /// <summary>
        /// Returns UserViewModel by login and password
        /// </summary>
        [HttpGet]
        [ActionName("GetUser")]
        public UserViewModel GetUser([FromQuery] string login, [FromQuery] string password)
        {
            UserViewModel result = default;
            try
            {
                result = _mapper.Map<UserViewModel>(_userService.GetUser(login, password));
                _logger.LogInformation($"User with \"{login}\" login has been given");

            }
            catch (Exception e)
            {
                _logger.LogWarning($"Attempt of giving user with \"{login}\" login   has been failed with following exception: {e}");
            }
            return result;
        }

        /// <summary>
        /// Validates if user with specific login and password exists
        /// </summary>
        [HttpGet]
        [ActionName("ValidateUser")]
        public bool ValidateUser([FromQuery] string login, [FromQuery] string password)
        {
            bool result = default;
            try
            {
                result = (_userService.GetUser(login, password) != null);
                _logger.LogInformation($"User validation with \"{login}\" login accomplished with \"{result}\" result");
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Attempt of user validation with \"{login}\" login has been failed with following exception: {e}");
            }
            return result;
        }

        /// <summary>
        /// Returns name of User's position
        /// </summary>
        [HttpGet]
        [ActionName("GetUserPosition")]
        public string GetUserPosition([FromQuery] int id) => _userService.GetUserPosition(id);

        /// <summary>
        /// Returns id of User's position
        /// </summary>
        [HttpGet]
        [ActionName("GetUserPositionId")]
        public int GetUserPositionId([FromQuery] int id) => _userService.GetUserPositionId(id);

        /// <summary>
        /// Returns all Users
        /// </summary>
        [HttpGet]
        [ActionName("GetAllUsers")]
        public IEnumerable<UserViewModel> GetAllUsers() 
            => _userService.GetAllUsers().Select(element => _mapper.Map<UserViewModel>(element));

        /// <summary>
        /// Adds new user
        /// </summary>
        [HttpPost]
        [ActionName("AddUser")]
        public void AddUser([FromBody] UserViewModel user)
        {
            try
            {
                _userService.AddUser(user);
                _logger.LogInformation($"New user with \"{user.Login}\" login has been added.");
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Attempt of adding the new user with \"{user.Login}\" failed with following exception: {e}");
            }
        }
    }
}