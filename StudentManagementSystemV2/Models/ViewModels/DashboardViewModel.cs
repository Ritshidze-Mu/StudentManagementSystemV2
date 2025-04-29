using System;
using System.Collections.Generic;
using StudentManagementSystemV2.Models;

namespace StudentManagementSystemV2.Models.ViewModels
{
    public class DashboardViewModel
    {
        public string UserName { get; set; }
        public bool HasCompletedPatientForm { get; set; }
        public List<Appointment> UpcomingAppointments { get; set; }
        public int PastAppointmentsCount { get; set; }
        public List<ActivityItem> RecentActivities { get; set; }
        public List<Diagnosis> RecentDiagnoses { get; set; }
        public List<Prescription> ActivePrescriptions { get; set; }
        public List<MedicalHistoryItem> MedicalHistory { get; set; }
    }

    public class ActivityItem
    {
        public string Icon { get; set; }
        public string IconColor { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}