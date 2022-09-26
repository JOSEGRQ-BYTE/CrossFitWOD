using System;
using CrossFitWOD.Interfaces;
using CrossFitWOD.Models;
using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrossFitWOD.Controllers
{
    [ApiController]
    [Route("api/Emails")]
    //[Authorize(Roles = "Administrator")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _EmailService;

        public EmailController(IEmailService emailService)
        {
            _EmailService = emailService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> SendEmailRequest([FromBody] EmailRequest userRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _EmailService.SendPlainTextMessageAsync(userRequest);
                    return "Thanks for reaching out! I will be in contact with you soon!";
                }
                catch (Exception error)
                {
                    return StatusCode(500, "Unknown error ocurred while sending message. Try again later!");
                }
            }

            return BadRequest();
        }
    }
}

