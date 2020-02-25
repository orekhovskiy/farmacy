using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Farmacy.Commons;
using Farmacy.Providers;
using Farmacy.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        [ActionName("ValidateUser")]
        public async Task<dynamic> ValidateUser([FromQuery] string login, [FromQuery] string password)
        {
            var identity = await GetIdentity(login, password);
            if (identity != null)
            {
                var now = DateTime.UtcNow;
                var jwt = new JwtSecurityToken(
                   issuer: AuthOptions.ISSUER,
                   audience: AuthOptions.AUDIENCE,
                   notBefore: now,
                   claims: identity.Claims,
                   expires: new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 23, 59, 59),
                   signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                var response = new
                {
                    access_token = encodedJwt,
                    username = identity.Name,
                };

                return response;
            }
            else
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
        }

        private async Task<ClaimsIdentity> GetIdentity(string login, string password)
        {
            var user = await _userApiProvider.GetUser(login, password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Position)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }

        /*[HttpGet]
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