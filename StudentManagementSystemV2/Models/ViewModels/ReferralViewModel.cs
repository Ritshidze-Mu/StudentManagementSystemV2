using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentManagementSystemV2.Models.ViewModels
{
    public class ReferralViewModel
    {
        public int AppointmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentNumber { get; set; }
        [Required]
        public string ReferredTo { get; set; }

        [Required]
        public string Reason { get; set; }
        public string ReasonForReferral { get; set; }     // ✅ Add this
        public string MedicalHistory { get; set; }        // ✅ Add this
        public string ReferredToDepartment { get; set; }
        public int NurseId { get; set; }
    }
}