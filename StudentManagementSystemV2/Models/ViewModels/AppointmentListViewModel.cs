using StudentManagementSystemV2.Models;
using System;
using System.Collections.Generic;

namespace StudentManagementSystemV2.Models.ViewModels
{
    public class AppointmentListViewModel
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string Reason { get; set; }
        public ApplicationUser User { get; set; }
        public string FilterStatus { get; set; }

        // If you need to keep the list for other views
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}