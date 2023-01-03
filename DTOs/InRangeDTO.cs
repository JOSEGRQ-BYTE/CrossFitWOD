using System;
using System.ComponentModel.DataAnnotations;

namespace CrossFitWOD.DTOs
{
	public class InRangeDTO
	{
		[Required]
		public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }
    }
}

