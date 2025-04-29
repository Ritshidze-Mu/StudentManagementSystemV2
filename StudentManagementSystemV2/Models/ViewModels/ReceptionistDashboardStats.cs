using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManagementSystemV2.Models.ViewModels
{
    public class ReceptionistDashboardStats
    {
        public int TodaysAppointmentsCount { get; set; }
        public int PendingCheckInsCount { get; set; }
        public int NoShowsCount { get; set; }
    }
}