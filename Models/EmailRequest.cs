using System;
using System.ComponentModel.DataAnnotations;

namespace CrossFitWOD.Models
{
    public class EmailRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }
        //public List<IFormFile> Attachments { get; set; }
    }
}

