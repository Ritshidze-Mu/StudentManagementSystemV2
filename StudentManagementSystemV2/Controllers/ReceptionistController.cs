using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using StudentManagementSystemV2.Models;
using StudentManagementSystemV2.Models.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StudentManagementSystemV2.Controllers
{
    [Authorize(Roles = "Receptionist")]
    public class ReceptionistController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        // GET: Receptionist
        public ActionResult Index(string filterStatus = null)
        {
            var today = DateTime.Today;

            var model = new AppointmentListViewModel
            {
                Appointments = db.Appointments
                    .Include(a => a.User)
                    .Include(a => a.Nurse)
                    .Where(a => DbFunctions.TruncateTime(a.AppointmentDate) == today) // Today's date filter
                    .Where(a => filterStatus == null || a.Status == filterStatus) // Filter by status if provided
                    .OrderBy(a => a.AppointmentTime)
                    .ToList(),
                FilterStatus = filterStatus
            };

            return View(model);
        }
        public ActionResult ManageCancellations()
        {
            var today = DateTime.Today;
            var model = new AppointmentListViewModel
            {
                Appointments = db.Appointments
                    .Include(a => a.User)
                    .Include(a => a.Nurse)
                    .Where(a => DbFunctions.TruncateTime(a.AppointmentDate) >= today)
                    .Where(a => a.Status == "Confirmed" )
                    .OrderBy(a => a.AppointmentDate)
                    .ThenBy(a => a.AppointmentTime)
                    .ToList()
            };
            return View(model);
        }
        public ActionResult Dashboard()
        {
            var today = DateTime.Today;
            var model = new ReceptionistDashboardViewModel
            {
                TodaysAppointmentsCount = db.Appointments.Count(a => DbFunctions.TruncateTime(a.AppointmentDate) == today),
                PendingCheckInsCount = db.Appointments.Count(a => DbFunctions.TruncateTime(a.AppointmentDate) == today && a.Status == "Confirmed"),
                NoShowsCount = db.Appointments.Count(a => DbFunctions.TruncateTime(a.AppointmentDate) == today && a.Status == "NoShow"),
                TodayAppointments = db.Appointments
                    .Where(a => DbFunctions.TruncateTime(a.AppointmentDate) == today)
                    .OrderBy(a => a.AppointmentTime)
                    .ToList()
            };
            return View(model);
        }

        // Check-in patient
        [HttpPost]
        public ActionResult CheckIn(int id)
        {
            var appointment = db.Appointments.Find(id);
            if (appointment != null)
            {
                appointment.Status = "CheckedIn";
                appointment.CheckInTime = DateTime.Now;
                db.SaveChanges();
                TempData["Message"] = "Patient checked in successfully";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Cancel(int id)
        {
            var appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        [HttpPost]
        public ActionResult Cancel(int id, string reason)
        {
            var appointment = db.Appointments.Find(id);
            if (appointment != null)
            {
                appointment.Status = "Cancelled"; // Mark appointment as cancelled
                appointment.CancellationReason = reason; // Store cancellation reason
                db.SaveChanges();
                TempData["SuccessMessage"] = "Appointment cancelled successfully";
            }
            return RedirectToAction("ManageCancellations");
        }

        public ActionResult Reschedule(int id)
        {
            var appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        [HttpPost]
        public ActionResult Reschedule(int id, DateTime newDate, string timeString)
        {
            if (!TimeSpan.TryParse(timeString, out TimeSpan newTime))
            {
                TempData["ErrorMessage"] = "Invalid time format";
                return RedirectToAction("Reschedule", new { id });
            }

            var appointment = db.Appointments.Find(id);
            if (appointment != null)
            {
                appointment.AppointmentDate = newDate.Date;
                appointment.AppointmentTime = newDate.Date.Add(newTime);
                appointment.Status = "Rescheduled";
                db.SaveChanges();
                TempData["Message"] = "Appointment rescheduled successfully";
            }
            return RedirectToAction("Index");
        }

        // GET: /Receptionist/Login
        // GET: /Receptionist/Login
        [AllowAnonymous]
public ActionResult Login()
        {
            return View();
        }

        // POST: /Receptionist/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(ReceptionistLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Find user by email
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ViewBag.Error = "Invalid login attempt";
                return View(model);
            }

            // Verify user is active and is a receptionist
            if (!user.IsActive || !await UserManager.IsInRoleAsync(user.Id, "Receptionist"))
            {
                ViewBag.Error = "Access denied";
                return View(model);
            }

            // Attempt to log in
            var result = await SignInManager.PasswordSignInAsync(
                user.UserName,
                model.Password,
                isPersistent: false,
                shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:
                    // Update last login date
                    user.LastLoginDate = DateTime.Now;
                    await UserManager.UpdateAsync(user);
                    return RedirectToAction("Dashboard", "Receptionist");

                case SignInStatus.LockedOut:
                    return View("Lockout");

                case SignInStatus.Failure:
                default:
                    ViewBag.Error = "Invalid login attempt";
                    return View(model);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous] // Important for access without login
        public ActionResult DebugFixReceptionistUser()
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindByEmail("reception@clinic.com");

            if (user == null)
            {
                return Content("User not found!");
            }

            // Reset password
            var token = userManager.GeneratePasswordResetToken(user.Id);
            var resetResult = userManager.ResetPassword(user.Id, token, "Clinic@1234");

            // Ensure account is active
            user.IsActive = true;
            user.EmailConfirmed = true;
            var updateResult = userManager.Update(user);

            // Add to role if not already
            if (!userManager.IsInRole(user.Id, "Receptionist"))
            {
                userManager.AddToRole(user.Id, "Receptionist");
            }

            // Return debug info
            return Content($"User fixed!<br>" +
                          $"Password reset: {(resetResult.Succeeded ? "Success" : "Failed")}<br>" +
                          $"Account update: {(updateResult.Succeeded ? "Success" : "Failed")}<br>" +
                          $"Current roles: {string.Join(", ", userManager.GetRoles(user.Id))}");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }

                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}