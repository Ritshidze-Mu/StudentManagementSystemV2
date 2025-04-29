// Attributes/ValidateAppointmentTimeAttribute.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace StudentManagementSystemV2.Attributes
{
    public class ValidateAppointmentTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime appointmentTime)
            {
                var time = appointmentTime.TimeOfDay;
                var isWithinMorningHours = time >= new TimeSpan(8, 0, 0) && time < new TimeSpan(12, 0, 0);
                var isWithinAfternoonHours = time >= new TimeSpan(13, 0, 0) && time < new TimeSpan(16, 0, 0);

                if (!isWithinMorningHours && !isWithinAfternoonHours)
                {
                    return new ValidationResult("Appointments must be between 8:00 AM - 12:00 PM or 1:00 PM - 4:00 PM");
                }

                // Check if time is on a 30-minute interval
                if (time.Minutes % 30 != 0)
                {
                    return new ValidationResult("Appointments must be scheduled on the hour or half-hour");
                }
            }

            return ValidationResult.Success;
        }
    }
}