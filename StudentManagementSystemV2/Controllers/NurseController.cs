using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using StudentManagementSystemV2.Models;
using StudentManagementSystemV2.Models.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StudentManagementSystemV2.Controllers
{
    [Authorize(Roles = "Nurse")]
    public class NurseController : Controller
    {

        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser, string> signInManager;

        public NurseController()
        {
            // Remove initialization from constructor
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(NurseLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Initialize managers
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var signInManager = new SignInManager<ApplicationUser, string>(
                userManager,
                HttpContext.GetOwinContext().Authentication);

            // Find user by email (not username)
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email address.");
                return View(model);
            }

            // Check if user is active
            if (!user.IsActive)
            {
                ModelState.AddModelError("", "Account is not active.");
                return View(model);
            }

            // Check if user is in the "Nurse" role
            if (!await userManager.IsInRoleAsync(user.Id, "Nurse"))
            {
                ModelState.AddModelError("", "You are not authorized as a nurse.");
                return View(model);
            }

            // Sign in using the email as username (since they're the same in your seed data)
            var result = await signInManager.PasswordSignInAsync(
                model.Email, // Use email as username
                model.Password,
                isPersistent: false,
                shouldLockout: false);

            if (result == SignInStatus.Success)
            {
                return RedirectToAction("Dashboard");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        private async Task SignInAsync(ApplicationUser user)
        {
            var auth = HttpContext.GetOwinContext().Authentication;
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var identity = await user.GenerateUserIdentityAsync(manager);
            auth.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);
        }

        public ActionResult Dashboard()
        {
            var nurse = GetLoggedInNurse();
            if (nurse == null) return RedirectToAction("Login");

            var today = DateTime.Today;
            var vm = new NurseDashboardViewModel
            {
                NurseName = nurse.Name,
                TodaysAppointments = db.Appointments
                    .Include(a => a.User)
                    .Where(a => a.NurseId == nurse.Id && DbFunctions.TruncateTime(a.AppointmentDate) == today)
                    .ToList(),

                UpcomingAppointments = db.Appointments
                    .Include(a => a.User)
                    .Where(a => a.NurseId == nurse.Id && DbFunctions.TruncateTime(a.AppointmentDate) > today)
                    .OrderBy(a => a.AppointmentDate)
                    .Take(5)
                    .ToList()
            };

            return View(vm);
        }

        public ActionResult ViewDetails(int id)
        {
            var appt = db.Appointments
                .Include(a => a.User)
                .Include(a => a.Nurse)
                .FirstOrDefault(a => a.Id == id);

            if (appt == null) return HttpNotFound();

            var patientForm = db.PatientForms
                .FirstOrDefault(p => p.UserId == appt.UserId);

            var diagnosis = db.Diagnoses.FirstOrDefault(d => d.AppointmentId == id);
            var prescription = db.Prescriptions.FirstOrDefault(p => p.AppointmentId == id);
            var referral = db.Referrals.FirstOrDefault(r => r.AppointmentId == id);

            var vm = new AppointmentDetailsViewModel
            {
                Appointment = appt,
                PatientForm = patientForm,
                NurseName = appt.Nurse?.Name ?? "Not Assigned",
                Diagnosis = diagnosis,
                Prescription = prescription,
                Referral = referral
            };

            return View(vm);
        }

        public ActionResult TodaysAppointments()
        {
            var nurse = GetLoggedInNurse();
            if (nurse == null) return RedirectToAction("Login");

            var today = DateTime.Today;
            var appointments = db.Appointments
                .Include(a => a.User)
                .Where(a => DbFunctions.TruncateTime(a.AppointmentDate) == today
                            && a.NurseId == nurse.Id)
                .OrderBy(a => a.AppointmentDate)
                .ToList();

            var vm = new NurseDashboardViewModel
            {
                NurseName = nurse.Name,
                TodaysAppointments = appointments
            };

            return View(vm);
        }

        public ActionResult UpcomingAppointments()
        {
            var nurse = GetLoggedInNurse();
            if (nurse == null) return RedirectToAction("Login");

            var tomorrow = DateTime.Today.AddDays(1);
            var appointments = db.Appointments
                .Include(a => a.User)
                .Where(a => DbFunctions.TruncateTime(a.AppointmentDate) >= tomorrow && a.NurseId == nurse.Id)
                .OrderBy(a => a.AppointmentDate)
                .ToList();

            return View(appointments);
        }

        public ActionResult DiagnosisSearch(string studentNumber)
        {
            var model = new DiagnosisSearchViewModel();
            model.StudentNumber = studentNumber;

            if (!string.IsNullOrEmpty(studentNumber))
            {
                var appointment = db.Appointments
                    .Include(a => a.User)
                    .FirstOrDefault(a =>
                        a.User.StudentNumber == studentNumber &&
                        DbFunctions.TruncateTime(a.AppointmentDate) == DbFunctions.TruncateTime(DateTime.Now)
                    );

                if (appointment != null)
                {
                    model.FoundAppointment = appointment;
                    model.ExistingDiagnosis = db.Diagnoses.FirstOrDefault(d => d.AppointmentId == appointment.Id);
                }
                else
                {
                    TempData["Error"] = "No appointment found for this student number today.";
                }
            }

            return View(model);
        }

        public ActionResult DiagnosisForm(int id)
        {
            var appt = db.Appointments
                .Include(a => a.User)
                .FirstOrDefault(a => a.Id == id);

            if (appt == null) return HttpNotFound();

            // Check if diagnosis already exists
            var existingDiagnosis = db.Diagnoses.FirstOrDefault(d => d.AppointmentId == id);
            if (existingDiagnosis != null)
            {
                TempData["Info"] = "This appointment already has a diagnosis.";
                return RedirectToAction("ViewDetails", new { id = id });
            }

            // Get patient form data
            var patientForm = db.PatientForms
                .FirstOrDefault(p => p.UserId == appt.UserId);

            var model = new DiagnosisViewModel
            {
                AppointmentId = appt.Id,
                FirstName = appt.User.FirstName,
                LastName = appt.User.LastName,
                StudentNumber = patientForm?.StudentNumber,
                NurseId = GetLoggedInNurse()?.Id ?? 0
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DiagnosisForm(DiagnosisViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate necessary fields including StudentNumber from PatientForm
                var appointment = db.Appointments
                    .Include(a => a.User)
                    .FirstOrDefault(a => a.Id == model.AppointmentId);

                if (appointment != null)
                {
                    var patientForm = db.PatientForms
                        .FirstOrDefault(p => p.UserId == appointment.UserId);

                    model.FirstName = appointment.User.FirstName;
                    model.LastName = appointment.User.LastName;
                    model.StudentNumber = patientForm?.StudentNumber;
                }

                TempData["Error"] = "Please correct the form errors and try again.";
                return View(model);
            }

            try
            {
                db.Diagnoses.Add(new Diagnosis
                {
                    AppointmentId = model.AppointmentId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    StudentNumber = model.StudentNumber,
                    BloodPressure = model.BloodPressure,
                    Temperature = model.Temperature,
                    Pulse = model.Pulse,
                    RespiratoryRate = model.RespiratoryRate,
                    ChiefComplaint = model.ChiefComplaint,
                    NursesAssessment = model.NursesAssessment,
                    DiagnosisDescription = model.DiagnosisDescription,
                    Plan = model.Plan,
                    NurseId = model.NurseId,
                    CreatedDate = DateTime.Now
                });

                db.SaveChanges();

                TempData["Success"] = "Diagnosis successfully saved!";
                return RedirectToAction("ViewDetails", new { id = model.AppointmentId });
            }
            catch (Exception ex)
            {
                // Log the error (ex)
                TempData["Error"] = "An error occurred while saving the diagnosis. Please try again.";
                return View(model);
            }
        }

        public ActionResult PrescriptionSearch(string studentNumber)
        {
            var model = new PrescriptionSearchViewModel { StudentNumber = studentNumber };

            if (!string.IsNullOrEmpty(studentNumber))
            {
                // Get today's appointment for this student
                var appointment = db.Appointments
                    .Include(a => a.User)
                    .FirstOrDefault(a =>
                        a.User.StudentNumber == studentNumber &&
                        DbFunctions.TruncateTime(a.AppointmentDate) == DbFunctions.TruncateTime(DateTime.Today));

                if (appointment != null)
                {
                    // Check if diagnosis exists (required before prescription)
                    var diagnosis = db.Diagnoses.FirstOrDefault(d => d.AppointmentId == appointment.Id);
                    if (diagnosis == null)
                    {
                        TempData["Error"] = "A diagnosis must be completed before creating a prescription.";
                        return View(model);
                    }

                    // Check if prescription already exists
                    var existingPrescription = db.Prescriptions.FirstOrDefault(p => p.AppointmentId == appointment.Id);
                    if (existingPrescription != null)
                    {
                        TempData["Info"] = "This appointment already has a prescription.";
                        model.ExistingPrescription = existingPrescription;
                    }

                    model.Appointment = appointment;
                    model.Diagnosis = diagnosis;
                }
                else
                {
                    TempData["Error"] = "No appointment found for this student number today.";
                }
            }

            return View(model);
        }

        public ActionResult PrescriptionForm(int id)
        {
            var appt = db.Appointments
                .Include(a => a.User)
                .FirstOrDefault(a => a.Id == id);

            if (appt == null) return HttpNotFound();

            // Check if diagnosis exists
            var diagnosis = db.Diagnoses.FirstOrDefault(d => d.AppointmentId == id);
            if (diagnosis == null)
            {
                TempData["Error"] = "You must complete a diagnosis before creating a prescription.";
                return RedirectToAction("DiagnosisForm", new { id = id });
            }

            // Check if prescription already exists
            var existingPrescription = db.Prescriptions.FirstOrDefault(p => p.AppointmentId == id);
            if (existingPrescription != null)
            {
                TempData["Info"] = "This appointment already has a prescription.";
                return RedirectToAction("ViewDetails", new { id = id });
            }

            // Get student number from patient form
            var patientForm = db.PatientForms.FirstOrDefault(p => p.UserId == appt.UserId);

            return View(new PrescriptionViewModel
            {
                AppointmentId = appt.Id,
                FirstName = appt.User.FirstName,
                LastName = appt.User.LastName,
                StudentNumber = patientForm?.StudentNumber,
                NurseId = GetLoggedInNurse()?.Id ?? 0
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrescriptionForm(PrescriptionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Debugging: Log all model errors
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                }
                // Repopulate student number if validation fails
                var appointment = db.Appointments
                    .Include(a => a.User)
                    .FirstOrDefault(a => a.Id == model.AppointmentId);

                if (appointment != null)
                {
                    var patientForm = db.PatientForms.FirstOrDefault(p => p.UserId == appointment.UserId);
                    model.StudentNumber = patientForm?.StudentNumber;
                }

                return View(model);
            }

            try
            {
                db.Prescriptions.Add(new Prescription
                {
                    AppointmentId = model.AppointmentId,
                    Medication = model.Medication,
                    Dosage = model.Dosage,
                    Frequency = model.Frequency,
                    Duration = model.Duration,
                    NurseId = model.NurseId,
                    DateIssued = DateTime.Now
                });

                db.SaveChanges();
                TempData["Success"] = "Prescription successfully saved!";
                return RedirectToAction("Dashboard", new { id = model.AppointmentId });
            }
            catch (Exception ex)
            {// Log the full error
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                TempData["Error"] = $"An error occurred: {ex.Message}";
                return View(model);
            }
        }

        public ActionResult ReferralSearch(string studentNumber)
        {
            var model = new ReferralSearchViewModel { StudentNumber = studentNumber };

            if (!string.IsNullOrEmpty(studentNumber))
            {
                // Get today's appointment for this student
                var appointment = db.Appointments
                    .Include(a => a.User)
                    .FirstOrDefault(a =>
                        a.User.StudentNumber == studentNumber &&
                        DbFunctions.TruncateTime(a.AppointmentDate) == DbFunctions.TruncateTime(DateTime.Today));

                if (appointment != null)
                {
                    // Check if diagnosis exists (required before referral)
                    var diagnosis = db.Diagnoses.FirstOrDefault(d => d.AppointmentId == appointment.Id);
                    if (diagnosis == null)
                    {
                        TempData["Error"] = "A diagnosis must be completed before creating a referral.";
                        return View(model);
                    }

                    // Check if prescription exists (can't refer if prescription exists)
                    var prescription = db.Prescriptions.FirstOrDefault(p => p.AppointmentId == appointment.Id);
                    if (prescription != null)
                    {
                        TempData["Error"] = "Cannot create referral for an appointment that already has a prescription.";
                        return View(model);
                    }

                    // Check if referral already exists
                    var existingReferral = db.Referrals.FirstOrDefault(r => r.AppointmentId == appointment.Id);
                    if (existingReferral != null)
                    {
                        TempData["Info"] = "This appointment already has a referral.";
                        model.ExistingReferral = existingReferral;
                    }

                    model.Appointment = appointment;
                    model.Diagnosis = diagnosis;
                }
                else
                {
                    TempData["Error"] = "No appointment found for this student number today.";
                }
            }

            return View(model);
        }

        public ActionResult ReferralForm(int id)
        {
            var appt = db.Appointments
                .Include(a => a.User)
                .FirstOrDefault(a => a.Id == id);

            if (appt == null) return HttpNotFound();

            // Check if diagnosis exists
            var diagnosis = db.Diagnoses.FirstOrDefault(d => d.AppointmentId == id);
            if (diagnosis == null)
            {
                TempData["Error"] = "You must complete a diagnosis before creating a referral.";
                return RedirectToAction("DiagnosisForm", new { id = id });
            }

            // Check if prescription exists (can't refer if prescription exists)
            var prescription = db.Prescriptions.FirstOrDefault(p => p.AppointmentId == id);
            if (prescription != null)
            {
                TempData["Error"] = "Cannot create referral for an appointment that already has a prescription.";
                return RedirectToAction("ViewDetails", new { id = id });
            }

            // Check if referral already exists
            var existingReferral = db.Referrals.FirstOrDefault(r => r.AppointmentId == id);
            if (existingReferral != null)
            {
                TempData["Info"] = "This appointment already has a referral.";
                return RedirectToAction("ViewDetails", new { id = id });
            }

            // Get student number from patient form
            var patientForm = db.PatientForms.FirstOrDefault(p => p.UserId == appt.UserId);

            return View(new ReferralViewModel
            {
                AppointmentId = appt.Id,
                FirstName = appt.User.FirstName,
                LastName = appt.User.LastName,
                StudentNumber = patientForm?.StudentNumber,
                NurseId = GetLoggedInNurse()?.Id ?? 0
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReferralForm(ReferralViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate student number if validation fails
                var appointment = db.Appointments
                    .Include(a => a.User)
                    .FirstOrDefault(a => a.Id == model.AppointmentId);

                if (appointment != null)
                {
                    var patientForm = db.PatientForms.FirstOrDefault(p => p.UserId == appointment.UserId);
                    model.StudentNumber = patientForm?.StudentNumber;
                }

                return View(model);
            }

            try
            {
                db.Referrals.Add(new Referral
                {
                    AppointmentId = model.AppointmentId,
                    ReferredToDepartment = model.ReferredToDepartment,
                    ReasonForReferral = model.ReasonForReferral,
                    MedicalHistory = model.MedicalHistory,
                    NurseId = model.NurseId,
                    DateReferred = DateTime.Now
                });

                db.SaveChanges();
                TempData["Success"] = "Referral successfully saved!";
                return RedirectToAction("ViewDetails", new { id = model.AppointmentId });
            }
            catch (Exception ex)
            {
                // Log the error (ex)
                TempData["Error"] = "An error occurred while saving the referral. Please try again.";
                return View(model);
            }
        }

        public ActionResult ViewBookingForm(int id)
        {
            var appointment = db.Appointments
                .Include(a => a.User)
                .FirstOrDefault(a => a.Id == id);

            if (appointment == null)
            {
                return HttpNotFound();
            }

            var viewModel = new AppointmentDetailsViewModel
            {
                Appointment = appointment,
                PatientForm = db.PatientForms.FirstOrDefault(p => p.UserId == appointment.UserId)
            };

            return View(viewModel);
        }

        private Nurse GetLoggedInNurse()
        {
            var userId = User.Identity.GetUserId();
            return db.Nurses.FirstOrDefault(n => n.ApplicationUserId == userId);
        }
      

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
