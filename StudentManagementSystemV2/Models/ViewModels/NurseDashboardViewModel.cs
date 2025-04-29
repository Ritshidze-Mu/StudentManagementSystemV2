using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManagementSystemV2.Models.ViewModels
{
    public class NurseDashboardViewModel
    {
        public string NurseName { get; set; }
        public int StudentId { get; set; }

        public List<Appointment> TodaysAppointments { get; set; } = new List<Appointment>();
        public List<Appointment> UpcomingAppointments { get; set; } = new List<Appointment>();


        // Use view models for form data
        public DiagnosisViewModel DiagnosisForm { get; set; } = new DiagnosisViewModel();
        public PrescriptionViewModel PrescriptionForm { get; set; } = new PrescriptionViewModel();
        public ReferralViewModel ReferralForm { get; set; } = new ReferralViewModel();

        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}