﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourseManagement.Models
{
    public class EnrollmentRequest
    {
        [Required]
        public string UserId { set; get; }

        [Required]
        public int CourseId { set; get; }
    }
}
