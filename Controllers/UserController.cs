using System;
using CrossFitWOD.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CrossFitWOD.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace CrossFitWOD.Controllers
{
    [ApiController]
    [Route("API/Users")]
    [EnableCors("AllowedSpecificationsOrigins")]
    public class UserController: ControllerBase
    {

        private readonly SignInManager<User> _SigninManager;
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly IConfiguration _Configuration;

        public UserController(IConfiguration configuration, SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _SigninManager = signInManager;
            _UserManager = userManager;
            _Configuration = configuration;
            _RoleManager = roleManager;
        }



        [HttpPost("SignUpUser")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<SignUpDTO>> SignUpUser([FromBody] SignUpDTO userForm)
        {
            // ONLY if model is valid
            if (ModelState.IsValid)
            {
                
                var existingUser = await _UserManager.FindByEmailAsync(userForm.Email);
                if (existingUser == null)
                {
                    User newUser = new User
                    {
                        UserName = userForm.Email,
                        Email = userForm.Email,
                        FirstName = userForm.FirstName,
                        LastName = userForm.LastName,
                        SecurityStamp = Guid.NewGuid().ToString(),
                    };

                    var newlyCreatedUser = await _UserManager.CreateAsync(newUser, userForm.Password);

                    if (newlyCreatedUser.Succeeded)
                    {
                        await _UserManager.AddToRoleAsync(newUser, "Admin");
                        return Created("", userForm);
                    }
                }

            }

            return BadRequest();
        }

        [HttpPost("LoginUser")]
        public async Task<ActionResult> LoginUser(LoginDTO userDetails)
        {
            // ONLY if model valid
            if (ModelState.IsValid)
            {
                var existingUser = await _UserManager.FindByEmailAsync(userDetails.Email);
                if (existingUser is not null)
                {
                    var passwordCheck = await _SigninManager.CheckPasswordSignInAsync(existingUser, userDetails.Password, false);
                    if (passwordCheck.Succeeded)
                    {
                        var userRoles = await _UserManager.GetRolesAsync(existingUser);

                        // Create User Claims
                        var authclaims = new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.Email, existingUser.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, existingUser.UserName),
                            new Claim(JwtRegisteredClaimNames.GivenName, existingUser.FirstName),
                            new Claim(JwtRegisteredClaimNames.FamilyName, existingUser.LastName),
                            new Claim("UserID", existingUser.Id)
                        };

                        // Add Roles to Claims
                        foreach (var userRole in userRoles)
                        {
                            authclaims.Add(new Claim(ClaimTypes.Role, userRole));
                        }

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["JWT:Secret"]));
                        var userCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var userToken = new JwtSecurityToken(
                            issuer: _Configuration["JWT:Issuer"],
                            audience: _Configuration["JWT:Audience"],
                            claims: authclaims,
                            expires: DateTime.UtcNow.AddHours(3),
                            signingCredentials: userCredentials);

                        return StatusCode(200, new
                        {
                            email = existingUser.Email,
                            firstName = existingUser.FirstName,
                            lastName = existingUser.LastName,
                            token = new JwtSecurityTokenHandler().WriteToken(userToken),
                            expiration = userToken.ValidTo,
                            isLoggedIn = true
                        });
                    }
                    else
                    {
                        return StatusCode(400, "Incorrect password.");
                    }

                }
                else
                {
                    return StatusCode(400, "User does not exist.");
                }
            }

            return BadRequest();
        }
    }
}

