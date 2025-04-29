using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StudentManagementSystemV2.Models;
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace StudentManagementSystemV2.Migrations
{
    public class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            try
            {
                // Initialize managers
                var userManager = new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(context));
                var roleManager = new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(context));

                // Configure password policy
                userManager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 6,
                    RequireNonLetterOrDigit = false,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireUppercase = true,
                };

                // 1. Create roles first
                CreateRoleIfNotExists(roleManager, "Nurse");
                CreateRoleIfNotExists(roleManager, "Receptionist");

                // 2. Create users
                var nurse1 = CreateUser(userManager, "nurse1@dut4life.ac.za", "John", "Doe", "Nurse");
                var nurse2 = CreateUser(userManager, "nurse2@dut4life.ac.za", "Jane", "Smith", "Nurse");
                var nurse3 = CreateUser(userManager, "nurse3@dut4life.ac.za", "Michael", "Johnson", "Nurse");
                var nurse4 = CreateUser(userManager, "nurse4@dut4life.ac.za", "Emily", "Taylor", "Nurse");
                var receptionist = CreateUser(userManager, "receptionist@dut4life.ac.za", "Sarah", "Williams", "Receptionist");

                // 3. Create nurse entities
                CreateNurse(context, nurse1.Id, "John Doe", "General Health", 20);
                CreateNurse(context, nurse2.Id, "Jane Smith", "Women's Health",20);
                CreateNurse(context, nurse3.Id, "Michael Johnson", "Men's Health", 20);
                CreateNurse(context, nurse4.Id, "Emily Taylor", "General Health", 20);

                context.SaveChanges();
            }

            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var failure in ex.EntityValidationErrors)
                {
                    Console.WriteLine($"Entity of type \"{failure.Entry.Entity.GetType().Name}\" in state \"{failure.Entry.State}\" has the following validation errors:");

                    foreach (var error in failure.ValidationErrors)
                    {
                        Console.WriteLine($"- Property: {error.PropertyName}, Error: {error.ErrorMessage}");
                    }
                }

                throw; // rethrow to see stack trace after console output
            }
        }

        // Helper methods
        private void CreateRoleIfNotExists(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!roleManager.RoleExists(roleName))
            {
                var role = new IdentityRole(roleName);
                roleManager.Create(role);
            }
        }

        private ApplicationUser CreateUser(UserManager<ApplicationUser> userManager,
            string email, string firstName, string lastName, string role)
        {
            // Check if user already exists
            var existingUser = userManager.FindByEmail(email);
            if (existingUser != null)
                return existingUser;

            // Create new user
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                EmailConfirmed = true,
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                RegistrationDate = DateTime.Now,
                IsActive = true
            };

            // Create user with password
            var result = userManager.Create(user, "Password123!");
            if (!result.Succeeded)
            {
                throw new Exception($"User creation failed: {string.Join(", ", result.Errors)}");
            }

            // Add to role after user is created
            var roleResult = userManager.AddToRole(user.Id, role);
            if (!roleResult.Succeeded)
            {
                throw new Exception($"Role assignment failed: {string.Join(", ", roleResult.Errors)}");
            }

            return user;
        }

        private void CreateNurse(ApplicationDbContext context, string userId,
    string name, string specialization, int maxAppointments)
        {
            if (!context.Nurses.Any(n => n.ApplicationUserId == userId))
            {
                var user = context.Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                    throw new Exception("Associated user not found for nurse.");

                context.Nurses.Add(new Nurse
                {
                    Name = name,
                    Email = user.Email, // Add this line
                    Specialization = specialization,
                    MaxDailyAppointments = maxAppointments,
                    ApplicationUserId = userId
                });
            }
        }
    }
}
