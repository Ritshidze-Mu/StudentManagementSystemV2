// Models/Data/Student.cs
using StudentManagementSystemV2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystemV2.Models
{

    public class Student
    {
        [Key]
        public string UserId { get; set; }  // Use UserId as the primary key
                                            // Use UserId as the primary key

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 13)]
        [RegularExpression(@"^[0-9]*$")]
        public string IdNumber { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 8)]
        [RegularExpression(@"^[0-9]*$")]
        public string StudentNumber { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public virtual ApplicationUser User { get; set; }  // Navigation property
    }

}
