using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentManagementSystemV2.Models.ViewModels
{
    public class PrescriptionSearchViewModel
    {
        public string StudentNumber { get; set; }
        public Appointment Appointment { get; set; }
        public Diagnosis Diagnosis { get; set; }
        public Prescription ExistingPrescription { get; set; }
    }
}