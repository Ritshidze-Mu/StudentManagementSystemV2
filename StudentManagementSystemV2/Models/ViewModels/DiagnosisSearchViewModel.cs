using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManagementSystemV2.Models.ViewModels
{
    public class DiagnosisSearchViewModel
    {
        public string StudentNumber { get; set; }
        public Appointment FoundAppointment { get; set; }
        public Diagnosis ExistingDiagnosis { get; set; }
    }
}