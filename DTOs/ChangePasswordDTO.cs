using System;
using System.ComponentModel.DataAnnotations;

namespace CrossFitWOD.DTOs
{
    public class ChangePasswordDTO
    {
        [Required, DataType(DataType.Password), Display(Name = "Current Password")]
        public string? CurrentPassword { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "New Password")]
        public string? NewPassword { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Confirm new password"),
            Compare("NewPassword", ErrorMessage =
            "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}

