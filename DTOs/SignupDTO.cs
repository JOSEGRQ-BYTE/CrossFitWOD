using System;
using System.ComponentModel.DataAnnotations;

namespace CrossFitWOD.DTOs
{
    public record SignUpDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "Password needs to be between 8-255 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 8, ErrorMessage = "Password needs to be between 8-255 characters long.")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}

