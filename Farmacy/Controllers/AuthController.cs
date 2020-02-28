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
                    role = identity.Claims.Where(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType).FirstOrDefault().Value
                };

                return response;
            }
            else
            {
                return BadRequest();
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
    }
}