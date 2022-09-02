using System;
using System.ComponentModel.DataAnnotations;

namespace CrossFitWOD.DTOs
{
    public record LoginDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

