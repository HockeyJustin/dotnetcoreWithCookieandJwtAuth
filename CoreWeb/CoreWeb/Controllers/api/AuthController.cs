using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CoreWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CoreWeb.Controllers.api
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        // GET api/values
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, lockoutOnFailure: true);

            if(result.Succeeded)
            {
                var key = CoreWeb.AuthHelpers.Issuers.GetKey();
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);


                // Get the user and their roles. Add it to the claims ----------------------------------------
                var loggedInUser = await _signInManager.UserManager.FindByEmailAsync(user.Email);

                var roles = await _signInManager.UserManager.GetRolesAsync(loggedInUser);

                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.Email));

                if(roles != null && roles.Any())
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.First()));
                }
                // --------------------------------------------------------------------------------------------

                var tokenOptions = new JwtSecurityToken(
                    issuer: CoreWeb.AuthHelpers.Issuers.Issuer,
                    audience: CoreWeb.AuthHelpers.Issuers.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}