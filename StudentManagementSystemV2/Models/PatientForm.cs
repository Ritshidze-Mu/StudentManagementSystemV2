using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystemV2.Models
{
    public class PatientForm
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        // 1. Demographic Information
        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Student Number")]
        
        [RegularExpression(@"^\d{8}$", ErrorMessage = "Student number must be 8 digits")] // Optional: enforce numbers only
        public string StudentNumber { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        // 2. Address Information
        [Required(ErrorMessage = "Street Address is required")]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Province is required")]
        public string Province { get; set; }

        [Required(ErrorMessage = "Postal Code is required")]
        [Display(Name = "Postal Code")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Postal Code must be 4 digits")]
        public string PostalCode { get; set; }

        // 3. Contact Information
        [Required(ErrorMessage = "Phone Number is required")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone Number must be 10 digits starting with 0")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression(@"^.+@dut4life\.ac\.za$", ErrorMessage = "Email must end with @dut4life.ac.za")]
        public string Email { get; set; }

        // 4. Medical History
        [Display(Name = "Allergies")]
        public string Allergies { get; set; } = "None";

        [Display(Name = "Current Medications")]
        public string CurrentMedications { get; set; } = "None";

        [Display(Name = "Pre-existing Conditions")]
        public string PreExistingConditions { get; set; } = "None";

        // 5. Disability Information
        [Required(ErrorMessage = "Please specify disability status")]
        [Display(Name = "Any Disability/Accessibility Requirements?")]
        public bool HasDisability { get; set; } = false;

        [Display(Name = "Disability Details")]
        public string DisabilityDetails { get; set; }

        // 6. Emergency Contact
        [Required(ErrorMessage = "Emergency Contact Name is required")]
        [Display(Name = "Full Name")]
        public string EmergencyContactName { get; set; }

        [Required(ErrorMessage = "Relationship is required")]
        [Display(Name = "Relationship to Patient")]
        public string EmergencyContactRelationship { get; set; }

        [Required(ErrorMessage = "Emergency Phone is required")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone Number must be 10 digits starting with 0")]
        public string EmergencyContactPhone { get; set; }

        [Display(Name = "Email (Optional)")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmergencyContactEmail { get; set; }

        // 7. Consent
        [Required(ErrorMessage = "You must consent to treatment")]
        [Display(Name = "Consent for Treatment")]
        public bool ConsentForTreatment { get; set; }

        [Required(ErrorMessage = "You must agree to confidentiality")]
        [Display(Name = "Confidentiality Agreement")]
        public bool ConfidentialityAgreement { get; set; }

        [Required(ErrorMessage = "You must select at least one communication method")]
        [Display(Name = "Preferred Communication Method")]
        public string CommunicationMethod { get; set; } // Will be a select list

        [Required(ErrorMessage = "You must consent to student-provided care")]
        [Display(Name = "Consent for Student-Provided Care")]
        public bool ConsentForStudentCare { get; set; }

        // Metadata
        public DateTime SubmissionDate { get; set; } = DateTime.Now;
        public bool IsCompleted { get; set; } = true;
    }
}