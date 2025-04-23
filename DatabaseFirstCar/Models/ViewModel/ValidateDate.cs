using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DatabaseFirstCar.Models.ViewModel
{
    public class ValidateDate:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           
            DateTime dateOfBirth;
            string message = "You must be at least 18 years old.";

            if (value == null || !DateTime.TryParse(value.ToString(), out dateOfBirth))
            {
                return new ValidationResult("You must be at least 18 years old to make a order");
            }

            DateTime today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;

            if (dateOfBirth > today.AddYears(-age))
            {
                age--;
            }

            if (age >= 18)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(message);
            }
        }
    }
}