using System;
using StudentManagementSystemV2.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace StudentManagementSystemV2.Models.ViewModels
{
    public class StudentViewModel
    {
        // Personal Information
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(50, ErrorMessage = "Surname cannot exceed 50 characters")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; } = DateTime.Today.AddYears(-18);

        [Required(ErrorMessage = "Gender is required")]
        public string SelectedGender { get; set; }
        public IEnumerable<SelectListItem> GenderOptions { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Male", Text = "Male" },
            new SelectListItem { Value = "Female", Text = "Female" },
            new SelectListItem { Value = "Other", Text = "Other" }
        };

        // Identification
        [Required(ErrorMessage = "ID Number is required")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "ID Number must be exactly 13 digits")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "ID Number must contain only digits")]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "Student Number is required")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Student Number must be exactly 8 digits")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Student Number must contain only digits")]
        public string StudentNumber { get; set; }

        // Account Information
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@dut4life\.ac\.za$",
            ErrorMessage = "Email must be a valid @dut4life.ac.za address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone Number must be exactly 10 digits")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Phone Number must contain only digits")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        // Method to convert ViewModel to Entity
        public Student ToEntity()
        {
            return new Student
            {
                Name = this.Name,
                Surname = this.Surname,
                DateOfBirth = this.DateOfBirth,
                Gender = this.SelectedGender,
                IdNumber = this.IdNumber,
                StudentNumber = this.StudentNumber,
                Email = this.Email,
                PhoneNumber = this.PhoneNumber
            };
        }
    }
}
