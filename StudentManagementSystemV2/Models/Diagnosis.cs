using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentManagementSystemV2.Models
{
    public class Diagnosis
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string StudentNumber { get; set; }
        public string BloodPressure { get; set; }
        public string Temperature { get; set; }
        public string Pulse { get; set; }
        public string RespiratoryRate { get; set; }
        public string ChiefComplaint { get; set; }
        public string NursesAssessment { get; set; }
        public string DiagnosisDescription { get; set; }  // Renamed property
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;


        public string Plan { get; set; }
        public int NurseId { get; set; }
      
        public virtual Nurse Nurse { get; set; }
        [ForeignKey("AppointmentId")]
        public virtual Appointment Appointment { get; set; }
   
    }
}