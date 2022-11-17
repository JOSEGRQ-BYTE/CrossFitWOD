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
using CrossFitWOD.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using System.Web;
using YamlDotNet.Core.Tokens;

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
        private readonly IEmailService _EmailService;

        public UserController(IConfiguration configuration, SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            _SigninManager = signInManager;
            _UserManager = userManager;
            _Configuration = configuration;
            _RoleManager = roleManager;
            _EmailService = emailService;
        }

        /*
         
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

                    //var token = await _UserManager.GenerateEmailConfirmationTokenAsync(newUser);
                    //var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = newUser.Email }, Request.Scheme);
                    //var message = new Message(new string[] { newUser.Email }, "Confirmation email link", confirmationLink, null);
                    //await _EmailService.SendEmailAsync(message);
                }

            }

            return BadRequest();
        }
         */

        [HttpPost("SignUpUser")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<string>> SignUpUser([FromBody] SignUpDTO userForm)
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
                        await _UserManager.AddToRoleAsync(newUser, userForm.Role);

                        var token = await _UserManager.GenerateEmailConfirmationTokenAsync(newUser);
                        token = HttpUtility.UrlEncode(token);
                        //var confirmationLink = Url.Action(nameof(ConfirmEmail), "User", new { token, email = newUser.Email }, Request.Scheme);

                        var stringParams = new Dictionary<string, string>()
                        {
                            { "token", token },
                            { "email", newUser.Email }
                        };

                        var confirmationLink = QueryHelpers.AddQueryString("http://localhost:4200/EmailVerification", stringParams);


                        await _EmailService.SendVerificationEmailAsync(confirmationLink, newUser.Email);

                        return Created("", userForm);
                    }
                }
                else
                {
                    return BadRequest("User already exists under this email. Please use diferent email or login.");
                }

            }

            return BadRequest("Invalid form.");
        }

        //token will last for two hours as well as the reset password token.
        [HttpGet("ConfirmEmail")]
        public async Task<ActionResult<string>> ConfirmEmail(string token, string email)
        {
            token = HttpUtility.UrlDecode(token);
            var user = await _UserManager.FindByEmailAsync(email);
            if (user is null)
                return BadRequest("Unable to find user with provided email.");

            IdentityResult result = await _UserManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return Ok();

            return BadRequest(result.Errors.FirstOrDefault()?.Description?? "Unable to confirm email.");
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
                            new Claim(JwtRegisteredClaimNames.Jti, existingUser.Id.ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, existingUser.UserName)
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
                            id = existingUser.Id,
                            email = existingUser.Email,
                            firstName = existingUser.FirstName,
                            lastName = existingUser.LastName,
                            token = new JwtSecurityTokenHandler().WriteToken(userToken),
                            expiration = userToken.ValidTo,
                            isLoggedIn = true,
                            isAdministrator = await _UserManager.IsInRoleAsync(existingUser, "Administrator")
                        });
                    }
                    else if (passwordCheck.IsNotAllowed)
                    {
                        return StatusCode(400, "Please make sure to verify your email before login in.");
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

        [HttpPost("ChangePassword")]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<string>> ChangePassword(ChangePasswordDTO passwordModel)
        {
            // If valid
            if (ModelState.IsValid)
            {
                // Pull current user
                var userId = HttpContext.User.Claims.First(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
                var user = await _UserManager.FindByIdAsync(userId);

                if (user is not null)
                {
                    var result = await _UserManager.ChangePasswordAsync(user, passwordModel.CurrentPassword, passwordModel.NewPassword);

                    // Password change was not successful
                    if (!result.Succeeded)
                    {
                        //List<string> errors = new List<string>();
                        //foreach (var error in result.Errors)
                        //{
                        //    errors.Add(error.Description);
                        //    //ModelState.AddModelError(string.Empty, error.Description);
                        //}

                        return BadRequest(result.Errors.FirstOrDefault()?.Description ?? "Unexpected Error");
                    }

                    await _SigninManager.RefreshSignInAsync(user);
                    return Ok();
                }
                else
                    return NotFound("User not found");
            }


            return BadRequest("Check your input");
        }

        [HttpGet("ForgotPassword")]
        public async Task<ActionResult<string>> ForgotPassword(string email)
        {
            if (String.IsNullOrEmpty(email))
                return BadRequest("Email field must not be empty.");

            var user = await _UserManager.FindByEmailAsync(email);

            if (user is null)
                return NotFound("User not found under email provided.");

            var token = await _UserManager.GeneratePasswordResetTokenAsync(user);
            token = HttpUtility.UrlEncode(token);

            var stringParams = new Dictionary<string, string>()
            {
                { "token", token },
                { "email", user.Email }
            };

            var confirmationLink = QueryHelpers.AddQueryString("http://localhost:4200/ResetPassword", stringParams);


            await _EmailService.SendResetPasswordLinkAsync(confirmationLink, user);

            return Ok();

        }

        [HttpPost("ResetPassword")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<string>> ResetPassword(ResetPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = await _UserManager.FindByEmailAsync(model.Email);
                if (user is null)
                    return NotFound("User not found under email provided.");

                var result = await _UserManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(model.Token), model.Password);
                if (result.Succeeded)
                    return Ok();

                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            }

            return BadRequest("Invaid form.");
        }
    }
}

