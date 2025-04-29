using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace StudentManagementSystemV2.Models.ViewModels
{
    public class PatientFormViewModel
    {
        // Demographic Information
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Student Number")]
        
        [RegularExpression(@"^\d{8}$", ErrorMessage = "Student number must be 8 digits")] // Optional
        public string StudentNumber { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; } = DateTime.Now.AddYears(-18);

        [Required]
        public string Gender { get; set; }
        public IEnumerable<SelectListItem> GenderOptions { get; set; }

        // Address Information
        [Required]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Province { get; set; }
        public IEnumerable<SelectListItem> ProvinceOptions { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        // Contact Information
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        // Medical History
        [Display(Name = "Allergies")]
        public string Allergies { get; set; } = "None";

        [Display(Name = "Current Medications")]
        public string CurrentMedications { get; set; } = "None";

        [Display(Name = "Pre-existing Conditions")]
        public string PreExistingConditions { get; set; } = "None";

        // Disability Information
        [Display(Name = "Any Disability/Accessibility Requirements?")]
        public bool HasDisability { get; set; } = false;

        [Display(Name = "Disability Details")]
        public string DisabilityDetails { get; set; }

        // Emergency Contact
        [Required]
        [Display(Name = "Full Name")]
        public string EmergencyContactName { get; set; }

        [Required]
        [Display(Name = "Relationship to Patient")]
        public string EmergencyContactRelationship { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string EmergencyContactPhone { get; set; }

        [Display(Name = "Email (Optional)")]
        public string EmergencyContactEmail { get; set; }

        // Consent
        [Required]
        [Display(Name = "Consent for Treatment")]
        public bool ConsentForTreatment { get; set; }

        [Required]
        [Display(Name = "Confidentiality Agreement")]
        public bool ConfidentialityAgreement { get; set; }

        [Required]
        [Display(Name = "Preferred Communication Method")]
        public string CommunicationMethod { get; set; }
        public IEnumerable<SelectListItem> CommunicationOptions { get; set; }

        [Required]
        [Display(Name = "Consent for Student-Provided Care")]
        public bool ConsentForStudentCare { get; set; }

        public bool IsEdit { get; set; } = false;
    }
}