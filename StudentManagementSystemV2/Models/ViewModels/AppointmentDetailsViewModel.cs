using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManagementSystemV2.Models.ViewModels
{
    public class AppointmentDetailsViewModel
    {
        internal Prescription Prescription;
        internal Referral Referral;

        public Appointment Appointment { get; set; }
        public PatientForm PatientForm { get; set; }
        public string NurseName { get; set; }
        public Diagnosis Diagnosis { get; internal set; }
    }
}