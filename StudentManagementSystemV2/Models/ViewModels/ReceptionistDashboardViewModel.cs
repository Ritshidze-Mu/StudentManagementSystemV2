using System.Collections.Generic;
using StudentManagementSystemV2.Models;

namespace StudentManagementSystemV2.Models.ViewModels
{
    public class ReceptionistDashboardViewModel
    {
        public int TodaysAppointmentsCount { get; set; }
        public int PendingCheckInsCount { get; set; }
        public int NoShowsCount { get; set; }
        public List<Appointment> TodayAppointments { get; set; }
    }
}