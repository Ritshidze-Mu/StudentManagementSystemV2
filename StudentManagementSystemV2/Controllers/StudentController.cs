using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using StudentManagementSystemV2.Models;
using StudentManagementSystemV2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StudentManagementSystemV2.Controllers
{
    public class StudentController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public StudentController()
        {
        }

        public StudentController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

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

        // GET: Student/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new StudentViewModel
            {
                DateOfBirth = DateTime.Today.AddYears(-18),
                GenderOptions = GetGenderOptions()
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check for existing student number or ID
                if (_db.Students.Any(s => s.StudentNumber == model.StudentNumber))
                {
                    ModelState.AddModelError("StudentNumber", "Student number already exists");
                    return View(model);
                }

                if (_db.Students.Any(s => s.IdNumber == model.IdNumber))
                {
                    ModelState.AddModelError("IdNumber", "ID number already registered");
                    return View(model);
                }

                // Create ApplicationUser
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.Name,
                    LastName = model.Surname,
                    PhoneNumber = model.PhoneNumber
                };

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Create Student record
                    var student = new Student
                    {
                        UserId = user.Id,
                        Name = model.Name,
                        Surname = model.Surname,
                        DateOfBirth = model.DateOfBirth,
                        Gender = model.SelectedGender,
                        IdNumber = model.IdNumber,
                        StudentNumber = model.StudentNumber
                    };

                    _db.Students.Add(student);
                    await _db.SaveChangesAsync();

                    // Add to Student role
                    await UserManager.AddToRoleAsync(user.Id, "Student");

                    // Sign in
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    TempData["SuccessMessage"] = $"Registration successful! Welcome {student.Name}";
                    return RedirectToAction("Login", "Account");
                }
                AddErrors(result);
            }
            return View("Register", new StudentViewModel());
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // Helper method for gender dropdown
        private List<SelectListItem> GetGenderOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "Male", Text = "Male" },
                new SelectListItem { Value = "Female", Text = "Female" },
                new SelectListItem { Value = "Other", Text = "Other" }
            };
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

                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}