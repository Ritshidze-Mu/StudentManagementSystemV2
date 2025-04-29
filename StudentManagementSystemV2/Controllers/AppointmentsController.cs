using Microsoft.AspNet.Identity;
using StudentManagementSystemV2.Models;
using StudentManagementSystemV2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace StudentManagementSystemV2.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // Constants for Clinic Times and Appointment Duration
        private static readonly TimeSpan ClinicOpenTime = TimeSpan.FromHours(8);    // 08:00
        private static readonly TimeSpan ClinicCloseTime = TimeSpan.FromHours(16);  // 16:00
        private static readonly TimeSpan LunchStartTime = TimeSpan.FromHours(12);   // 12:00
        private static readonly TimeSpan LunchEndTime = TimeSpan.FromHours(13);     // 13:00
        private static readonly int AppointmentDurationMinutes = 30;

        [HttpGet]
        public JsonResult GetTimeSlots(string dateString, string reason)
        {
            try
            {
                if (!DateTime.TryParse(dateString, out DateTime date))
                {
                    return Json(new { error = "Invalid date format" }, JsonRequestBehavior.AllowGet);
                }

                if (date < DateTime.Today)
                {
                    return Json(new { error = "Date cannot be in the past" }, JsonRequestBehavior.AllowGet);
                }

                var slots = GenerateAvailableTimeSlots(date, reason);
                return Json(slots, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetTimeSlots: {ex}");
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private List<object> GenerateAvailableTimeSlots(DateTime date, string reason)
        {
            var specialization = GetSpecializationFromReason(reason);

            // First get all nurses with the matching specialization
            var nurses = _db.Nurses
                .Where(n => n.Specialization == specialization)
                .ToList(); // Execute query immediately with ToList()

            var slots = new List<object>();

            // Get all appointments for the date
            var existingAppointments = _db.Appointments
                .Where(a => DbFunctions.TruncateTime(a.AppointmentDate) == date.Date)
                .ToList(); // Execute query immediately with ToList()

            foreach (var nurse in nurses)
            {
                var nurseAppointments = existingAppointments
                    .Where(a => a.NurseId == nurse.Id)
                    .ToList();

                var bookedTimes = nurseAppointments
                    .Select(a => a.AppointmentTime.TimeOfDay)
                    .ToList();

                for (TimeSpan currentTime = ClinicOpenTime;
                     currentTime < ClinicCloseTime;
                     currentTime = currentTime.Add(TimeSpan.FromMinutes(AppointmentDurationMinutes)))
                {
                    if (currentTime >= LunchStartTime && currentTime < LunchEndTime)
                        continue;

                    if (!bookedTimes.Contains(currentTime))
                    {
                        // Format the strings after the data is materialized
                        var displayTime = currentTime.ToString(@"hh\:mm");
                        var displayText = $"{displayTime} - {nurse.Name}";

                        slots.Add(new
                        {
                            Time = displayTime,
                            DisplayText = displayText,
                            NurseId = nurse.Id,
                            NurseName = nurse.Name
                        });
                    }
                }
            }

            return slots.OrderBy(s => ((dynamic)s).Time).ToList();
        }

        [HttpGet]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var patientForm = _db.PatientForms.FirstOrDefault(p => p.UserId == userId);

            if (patientForm == null)
            {
                TempData["ErrorMessage"] = "Please complete your patient information form before booking an appointment.";
                return RedirectToAction("Create", "PatientForm");
            }

            return View(new AppointmentViewModel
            {
                FirstName = patientForm.FirstName,
                LastName = patientForm.LastName,
                StudentNumber = patientForm.StudentNumber,
                ConsentForTreatment = patientForm.ConsentForTreatment,
                ReasonOptions = GetReasonOptions(),
                AvailableNurses = GetNurseOptions()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!TimeSpan.TryParse(model.SelectedTimeSlot, out TimeSpan selectedTime))
                {
                    ModelState.AddModelError("SelectedTimeSlot", "Please select a valid time slot");
                    return View(model);
                }

                var appointment = new Appointment
                {
                    UserId = User.Identity.GetUserId(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    StudentNumber = model.StudentNumber,
                    AppointmentDate = model.AppointmentDate.Date,
                    AppointmentTime = model.AppointmentDate.Date.Add(selectedTime),
                    NurseId = model.SelectedNurseId,
                    Reason = model.Reason,
                    AdditionalInfo = model.AdditionalInfo,
                    Status = "Confirmed"
                };

                _db.Appointments.Add(appointment);
                _db.SaveChanges();

                Task.Run(() => SendConfirmationEmail(appointment));
                TempData["SuccessMessage"] = "Appointment booked successfully!";
                return RedirectToAction("Index", "Dashboard");
            }

            model.ReasonOptions = GetReasonOptions();
            model.AvailableNurses = GetNurseOptions();
            return View(model);
        }

        private List<SelectListItem> GetReasonOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "General Checkup", Text = "General Checkup" },
                new SelectListItem { Value = "Women's Health", Text = "Women's Health" },
                new SelectListItem { Value = "Men's Health", Text = "Men's Health" }
            };
        }

        private List<SelectListItem> GetNurseOptions()
        {
            var nurses = _db.Nurses.ToList(); // Materialize data first

            return nurses.Select(n => new SelectListItem
            {
                Value = n.Id.ToString(),
                Text = $"{n.Name} ({n.Specialization})" // now safe
            }).ToList();
        }
        private string GetSpecializationFromReason(string reason)
        {
            if (string.IsNullOrEmpty(reason)) return "General Health";
            if (reason.Contains("Women")) return "Women's Health";
            if (reason.Contains("Men")) return "Men's Health";
            return "General Health";
        }

        private void SendConfirmationEmail(Appointment appointment)
        {
            try
            {
                var user = _db.Users.Find(appointment.UserId);
                if (user == null || string.IsNullOrWhiteSpace(user.Email)) return;

                using (var mailMessage = new MailMessage())
                using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    mailMessage.From = new MailAddress("your-email@gmail.com");
                    mailMessage.To.Add(user.Email);
                    mailMessage.Subject = "Appointment Confirmation";
                    mailMessage.Body = $"Dear {appointment.FirstName},\n\n" +
                                     $"Your appointment has been confirmed:\n\n" +
                                     $"Date: {appointment.AppointmentDate:dddd, MMMM dd, yyyy}\n" +
                                     $"Time: {appointment.AppointmentTime:h:mm tt}\n" +
                                     $"Reason: {appointment.Reason}\n\n" +
                                     $"Thank you!";
                    mailMessage.IsBodyHtml = false;

                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential(
                        ConfigurationManager.AppSettings["EmailUsername"],
                        ConfigurationManager.AppSettings["EmailPassword"]);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Email sending failed: {ex}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}