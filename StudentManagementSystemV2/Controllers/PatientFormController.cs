using Microsoft.AspNet.Identity;
using StudentManagementSystemV2.Models;
using StudentManagementSystemV2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace StudentManagementSystemV2.Controllers
{
    [Authorize]
    public class PatientFormController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: PatientForm/Create
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();

            // Check if form already exists
            if (_db.PatientForms.Any(p => p.UserId == userId))
            {
                return RedirectToAction("View"); // Redirect to view if form exists
            }

            // Pre-fill form with user data if available
            var user = _db.Users.Find(userId);
            var model = new PatientFormViewModel
            {
                FirstName = user?.FirstName ?? "",
                LastName = user?.LastName ?? "",
                Email = user?.Email ?? "",
                PhoneNumber = user?.PhoneNumber ?? "",
                GenderOptions = GetGenderOptions(),
                ProvinceOptions = GetProvinceOptions(),
                CommunicationOptions = GetCommunicationOptions()
            };

            return View(model);
        }

        // POST: PatientForm/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PatientFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var user = _db.Users.Find(userId);

                var patientForm = new PatientForm
                {
                    UserId = userId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    StudentNumber = model.StudentNumber,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    StreetAddress = model.StreetAddress,
                    City = model.City,
                    Province = model.Province,
                    PostalCode = model.PostalCode,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    Allergies = model.Allergies,
                    CurrentMedications = model.CurrentMedications,
                    PreExistingConditions = model.PreExistingConditions,
                    HasDisability = model.HasDisability,
                    DisabilityDetails = model.HasDisability ? model.DisabilityDetails : null,
                    EmergencyContactName = model.EmergencyContactName,
                    EmergencyContactRelationship = model.EmergencyContactRelationship,
                    EmergencyContactPhone = model.EmergencyContactPhone,
                    EmergencyContactEmail = model.EmergencyContactEmail,
                    ConsentForTreatment = model.ConsentForTreatment,
                    ConfidentialityAgreement = model.ConfidentialityAgreement,
                    CommunicationMethod = model.CommunicationMethod,
                    ConsentForStudentCare = model.ConsentForStudentCare,
                    IsCompleted = true,
                    SubmissionDate = DateTime.Now
                };

                _db.PatientForms.Add(patientForm);

                // Update user's form completion status
                user.HasCompletedPatientForm = true;
                _db.SaveChanges();

                TempData["SuccessMessage"] = "Patient form submitted successfully!";
                return RedirectToAction("Index", "Dashboard");
            }

            // If we got this far, something failed; redisplay form
            model.GenderOptions = GetGenderOptions();
            model.ProvinceOptions = GetProvinceOptions();
            model.CommunicationOptions = GetCommunicationOptions();
            return View(model);
        }

        // GET: PatientForm/Edit
        public ActionResult Edit()
        {
            var userId = User.Identity.GetUserId();
            var form = _db.PatientForms.FirstOrDefault(p => p.UserId == userId);

            if (form == null)
            {
                return RedirectToAction("Create");
            }

            var viewModel = new PatientFormViewModel
            {
                IsEdit = true,
                FirstName = form.FirstName,
                LastName = form.LastName,
                DateOfBirth = form.DateOfBirth,
                Gender = form.Gender,
                StreetAddress = form.StreetAddress,
                City = form.City,
                Province = form.Province,
                PostalCode = form.PostalCode,
                PhoneNumber = form.PhoneNumber,
                Email = form.Email,
                Allergies = form.Allergies,
                CurrentMedications = form.CurrentMedications,
                PreExistingConditions = form.PreExistingConditions,
                HasDisability = form.HasDisability,
                DisabilityDetails = form.DisabilityDetails,
                EmergencyContactName = form.EmergencyContactName,
                EmergencyContactRelationship = form.EmergencyContactRelationship,
                EmergencyContactPhone = form.EmergencyContactPhone,
                EmergencyContactEmail = form.EmergencyContactEmail,
                ConsentForTreatment = form.ConsentForTreatment,
                ConfidentialityAgreement = form.ConfidentialityAgreement,
                CommunicationMethod = form.CommunicationMethod,
                ConsentForStudentCare = form.ConsentForStudentCare,
                GenderOptions = GetGenderOptions(),
                ProvinceOptions = GetProvinceOptions(),
                CommunicationOptions = GetCommunicationOptions()
            };

            return View(viewModel);
        }

        // POST: PatientForm/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PatientFormViewModel model)
        {
            System.Diagnostics.Debug.WriteLine($"Received StudentNumber: '{model.StudentNumber}'");

            // Manually validate StudentNumber
            if (!string.IsNullOrWhiteSpace(model.StudentNumber))
            {
                model.StudentNumber = model.StudentNumber.Trim();
                if (model.StudentNumber.Length == 8 && System.Text.RegularExpressions.Regex.IsMatch(model.StudentNumber, @"^\d{8}$"))
                {
                    ModelState.Remove("StudentNumber");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.Identity.GetUserId();
                    var form = _db.PatientForms.FirstOrDefault(p => p.UserId == userId);

                    if (form == null)
                    {
                        return RedirectToAction("Create");
                    }

                    // Update all fields
                    form.FirstName = model.FirstName;
                    form.LastName = model.LastName;
                    form.StudentNumber = model.StudentNumber;
                    form.DateOfBirth = model.DateOfBirth;
                    form.Gender = model.Gender;
                    form.StreetAddress = model.StreetAddress;
                    form.City = model.City;
                    form.Province = model.Province;
                    form.PostalCode = model.PostalCode;
                    form.PhoneNumber = model.PhoneNumber;
                    form.Email = model.Email;
                    form.Allergies = model.Allergies;
                    form.CurrentMedications = model.CurrentMedications;
                    form.PreExistingConditions = model.PreExistingConditions;
                    form.HasDisability = model.HasDisability;
                    form.DisabilityDetails = model.HasDisability ? model.DisabilityDetails : null;
                    form.EmergencyContactName = model.EmergencyContactName;
                    form.EmergencyContactRelationship = model.EmergencyContactRelationship;
                    form.EmergencyContactPhone = model.EmergencyContactPhone;
                    form.EmergencyContactEmail = model.EmergencyContactEmail;
                    form.ConsentForTreatment = model.ConsentForTreatment;
                    form.ConfidentialityAgreement = model.ConfidentialityAgreement;
                    form.CommunicationMethod = model.CommunicationMethod;
                    form.ConsentForStudentCare = model.ConsentForStudentCare;
                    form.SubmissionDate = DateTime.Now;

                    _db.SaveChanges();

                    TempData["SuccessMessage"] = "Patient form updated successfully!";
                    return RedirectToAction("Index", "Dashboard");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    // Log the validation errors
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine(
                                $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
            // If we got this far, something failed; redisplay form
            model.GenderOptions = GetGenderOptions();
            model.ProvinceOptions = GetProvinceOptions();
            model.CommunicationOptions = GetCommunicationOptions();
            return View(model);
        }

        #region Helpers
        private SelectList GetGenderOptions()
        {
            return new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "Male", Text = "Male" },
                new SelectListItem { Value = "Female", Text = "Female" },
                new SelectListItem { Value = "Other", Text = "Other" }
            }, "Value", "Text");
        }

        private SelectList GetProvinceOptions()
        {
            return new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "Eastern Cape", Text = "Eastern Cape" },
                new SelectListItem { Value = "Free State", Text = "Free State" },
                new SelectListItem { Value = "Gauteng", Text = "Gauteng" },
                new SelectListItem { Value = "KwaZulu-Natal", Text = "KwaZulu-Natal" },
                new SelectListItem { Value = "Limpopo", Text = "Limpopo" },
                new SelectListItem { Value = "Mpumalanga", Text = "Mpumalanga" },
                new SelectListItem { Value = "Northern Cape", Text = "Northern Cape" },
                new SelectListItem { Value = "North West", Text = "North West" },
                new SelectListItem { Value = "Western Cape", Text = "Western Cape" }
            }, "Value", "Text");
        }

        private SelectList GetCommunicationOptions()
        {
            return new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "Phone", Text = "Phone" },
                new SelectListItem { Value = "Email", Text = "Email" },
                new SelectListItem { Value = "Both", Text = "Both Phone and Email" }
            }, "Value", "Text");
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
        // GET: PatientForm/View
        public ActionResult View()
        {
            var userId = User.Identity.GetUserId();
            var form = _db.PatientForms.FirstOrDefault(p => p.UserId == userId);

            if (form == null)
            {
                return RedirectToAction("Create");
            }

            var viewModel = new PatientFormViewModel
            {
                FirstName = form.FirstName,
                LastName = form.LastName,
                DateOfBirth = form.DateOfBirth,
                Gender = form.Gender,
                StreetAddress = form.StreetAddress,
                City = form.City,
                Province = form.Province,
                PostalCode = form.PostalCode,
                PhoneNumber = form.PhoneNumber,
                Email = form.Email,
                Allergies = form.Allergies,
                CurrentMedications = form.CurrentMedications,
                PreExistingConditions = form.PreExistingConditions,
                HasDisability = form.HasDisability,
                DisabilityDetails = form.DisabilityDetails,
                EmergencyContactName = form.EmergencyContactName,
                EmergencyContactRelationship = form.EmergencyContactRelationship,
                EmergencyContactPhone = form.EmergencyContactPhone,
                EmergencyContactEmail = form.EmergencyContactEmail,
                ConsentForTreatment = form.ConsentForTreatment,
                ConfidentialityAgreement = form.ConfidentialityAgreement,
                CommunicationMethod = form.CommunicationMethod,
                ConsentForStudentCare = form.ConsentForStudentCare,
                GenderOptions = GetGenderOptions(),
                ProvinceOptions = GetProvinceOptions(),
                CommunicationOptions = GetCommunicationOptions()
            };

            return View(viewModel);
        }
    }
}