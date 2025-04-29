using Microsoft.AspNet.Identity;
using StudentManagementSystemV2.Models;
using StudentManagementSystemV2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace StudentManagementSystemV2.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user = _db.Users.Find(userId);

            var appointments = _db.Appointments
                .Where(a => a.UserId == userId)
                .Include(a => a.Diagnoses)
                .Include(a => a.Prescriptions)
                .ToList();

            var upcomingAppointments = appointments
                .Where(a => a.AppointmentDate > DateTime.Now)
                .OrderBy(a => a.AppointmentDate)
                .ToList();

            var pastAppointmentsCount = appointments
                .Count(a => a.AppointmentDate <= DateTime.Now);

            var patientForm = _db.PatientForms.FirstOrDefault(p => p.UserId == userId);

            var model = new DashboardViewModel
            {
                UserName = user?.FullName ?? user?.UserName,  // use fallback
                HasCompletedPatientForm = patientForm != null,
                UpcomingAppointments = upcomingAppointments,
                PastAppointmentsCount = pastAppointmentsCount,
                RecentActivities = GetRecentActivities(userId),
                RecentDiagnoses = appointments
                    .SelectMany(a => a.Diagnoses)
                    .OrderByDescending(d => d.Appointment.AppointmentDate)
                    .Take(3)
                    .ToList(),
                ActivePrescriptions = appointments
                    .SelectMany(a => a.Prescriptions)
                    .OrderByDescending(p => p.Appointment.AppointmentDate)
                    .Take(3)
                    .ToList(),
                MedicalHistory = GetMedicalHistory(userId)
            };

            return View(model);
        }
        [Authorize]
        public ActionResult MedicalHistory()
        {
            var userId = User.Identity.GetUserId();
            var history = GetMedicalHistory(userId);
            return View(history);
        }

        [Authorize]
        public ActionResult Prescriptions()
        {
            var userId = User.Identity.GetUserId();
            var prescriptions = _db.Prescriptions
                .Where(p => p.Appointment.UserId == userId)
                .Include(p => p.Appointment)
                .OrderByDescending(p => p.Appointment.AppointmentDate)
                .ToList();

            return View(prescriptions);
        }

        [Authorize]
        public ActionResult Diagnoses()
        {
            var userId = User.Identity.GetUserId();
            var diagnoses = _db.Diagnoses
                .Where(d => d.Appointment.UserId == userId)
                .Include(d => d.Appointment)
                .OrderByDescending(d => d.Appointment.AppointmentDate)
                .ToList();

            return View(diagnoses);
        }
        private List<MedicalHistoryItem> GetMedicalHistory(string userId)
        {
            var history = new List<MedicalHistoryItem>();

            var appointments = _db.Appointments
                .Where(a => a.UserId == userId)
                .Include(a => a.Diagnoses)
                .Include(a => a.Prescriptions)
                .Include(a => a.Referrals)
                .ToList();

            foreach (var appointment in appointments)
            {
                if (appointment.Diagnoses != null)
                {
                    history.AddRange(appointment.Diagnoses.Select(d => new MedicalHistoryItem
                    {
                        Date = appointment.AppointmentDate,
                        Description = d.DiagnosisDescription,
                        Type = "Diagnosis"
                    }));
                }

                if (appointment.Prescriptions != null)
                {
                    history.AddRange(appointment.Prescriptions.Select(p => new MedicalHistoryItem
                    {
                        Date = appointment.AppointmentDate,
                        Description = $"{p.Medication} - {p.Dosage}",
                        Type = "Prescription"
                    }));
                }
            }

            return history.OrderByDescending(h => h.Date).Take(10).ToList();
        }

        public ActionResult Activities()
        {
            var userId = User.Identity.GetUserId();
            var activities = GetAllActivities(userId);
            return View(activities);
        }

        private List<ActivityItem> GetRecentActivities(string userId)
        {
            return GetAllActivities(userId, 3);
        }

        private List<ActivityItem> GetAllActivities(string userId, int maxCount = int.MaxValue)
        {
            var activities = new List<ActivityItem>();

            // Add appointment activities
            var recentAppointments = _db.Appointments
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .Take(maxCount)
                .ToList();

            foreach (var appt in recentAppointments)
            {
                activities.Add(new ActivityItem
                {
                    Icon = "calendar-check",
                    IconColor = appt.Status == "Cancelled" ? "danger" : "success",
                    Message = $"Appointment {appt.Status.ToLower()} for {appt.AppointmentDate:dd MMM}",
                    Timestamp = appt.CreatedAt
                });
            }

            // Add patient form activity if completed
            var patientForm = _db.PatientForms.FirstOrDefault(p => p.UserId == userId);
            if (patientForm != null)
            {
                activities.Add(new ActivityItem
                {
                    Icon = "file-alt",
                    IconColor = "info",
                    Message = "Patient information form submitted",
                    Timestamp = patientForm.SubmissionDate
                });
            }

            return activities.OrderByDescending(a => a.Timestamp).Take(maxCount).ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}