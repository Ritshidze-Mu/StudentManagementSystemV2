using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace StudentManagementSystemV2.Models.ViewModels
{
    public class AppointmentViewModel
    {
        // Patient Information
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "Student Number must be 8 digits")]
        public string StudentNumber { get; set; }

        // Appointment Details
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AppointmentDate { get; set; } = DateTime.Today;

        [Required]
        public string SelectedTimeSlot { get; set; }

        // List of available time slots for the appointment
        public List<SelectListItem> AvailableTimeSlots { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "-- Select Time --" }
        };

        // Reason for Visit
        [Required]
        public string Reason { get; set; }

        // List of reason options for the visit
        public List<SelectListItem> ReasonOptions { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "General Checkup", Text = "General Checkup" },
            new SelectListItem { Value = "Women's Health", Text = "Women's Health" },
            new SelectListItem { Value = "Men's Health", Text = "Men's Health" },
        };

        // Additional Information
        public string AdditionalInfo { get; set; }

        // Consent for the appointment details
        [Required(ErrorMessage = "You must confirm the appointment details")]
        public bool ConfirmedDetails { get; set; }

        // Consent for treatment
        public bool ConsentForTreatment { get; set; }

        // Selected Nurse Id (for when a nurse is selected for the appointment)
        [Required]
        public int SelectedNurseId { get; set; }

        // List of available nurses for the appointment

        public IEnumerable<SelectListItem> AvailableNurses { get; set; }
    }
}