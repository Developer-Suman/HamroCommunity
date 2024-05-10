using Project.BLL.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project.BLL.Validator
{
    public class RegistrationValidator
    {
        public static List<string> RegistrationValidate(RegistrationCreateDTOs registrationCreateDTOs)
        {
            var errors = new List<string>();
            if(registrationCreateDTOs is null)
            {
                errors.Add("Data Required for Registration is Null");
            }

            if (registrationCreateDTOs.Username is null || registrationCreateDTOs.Password is null || registrationCreateDTOs.Email is null || registrationCreateDTOs.Role is null)
            {
                errors.Add("Check each form and fill it properly");
            }

            if(!IsEmalValid(registrationCreateDTOs.Email))
            {
                errors.Add("Please provide Valid Email");

            }

            return errors;

        }

        private static bool IsEmalValid(string email)
        {
            var pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
    }
}
