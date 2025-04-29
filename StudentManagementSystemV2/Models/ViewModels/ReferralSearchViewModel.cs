using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManagementSystemV2.Models.ViewModels
{
    public class ReferralSearchViewModel
    {
        public string StudentNumber { get; set; }
        public Appointment Appointment { get; set; }
        public Diagnosis Diagnosis { get; set; }
        public Referral ExistingReferral { get; set; }

    }
}