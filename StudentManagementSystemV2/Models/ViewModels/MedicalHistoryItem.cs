using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManagementSystemV2.Models.ViewModels
{
    public class MedicalHistoryItem
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } // "Diagnosis", "Prescription", "Referral"
    }
}