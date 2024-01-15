using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using MyImage_API.DTOs;
using MyImage_API.Models.Mail;

namespace PR_API.Controllers
{
    public class MailController : Controller
    {
        private readonly MailService _mailService;

        public MailController(MailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("send")]
        public IActionResult SendMail([FromBody] EmailModel email)
        {
            // Dùng _mailService để gửi email
            _mailService.SendMail(email.To, email.Subject, email.Body);

            return Ok();
        }
        public void SendPasswordResetConfirmation(string to, string resetLink)
        {
            var subject = "Password Reset Confirmation";
            var body = $"Click the following link to reset your password: {resetLink}";

            _mailService.SendMail(to, subject, body);
        }

    }
}