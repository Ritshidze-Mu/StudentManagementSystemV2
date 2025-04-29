using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManagementSystemV2.Models
{
    public class Referral
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }  // Add AppointmentId
        public string ReferredToDepartment { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }// Pulled from appointment
        public string StudentNumber { get; set; }       // Pulled from appointment

        public string ReasonForReferral { get; set; }
        public string MedicalHistory { get; set; }
        public string AdditionalNotes { get; set; }

                       // To track the referring nurse
        public DateTime DateReferred { get; set; } = DateTime.Now;
        public virtual Appointment Appointment { get; set; }
        public int NurseId { get; set; }
        public virtual Nurse Nurse { get; set; }
    }
}