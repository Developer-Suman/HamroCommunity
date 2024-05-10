using Project.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project.BLL.Validator
{
    public class LogInValidator
    {
        public static List<string> LogInValidate(LogInDTOs logInDTOs)
        {
            var errors = new List<string>();

            if(logInDTOs is null)
            {
                errors.Add("Data Required For Login is Null");

            }

            if(string.IsNullOrEmpty(logInDTOs.Email))
            {
                errors.Add("Email is Required");
            }
            if(string.IsNullOrEmpty(logInDTOs.Password))
            {
                errors.Add("Password is Required");
            }

            if(!IsEmailValid(logInDTOs.Email))
            {
                errors.Add("Please provide with Valid Email");
            }

            return errors;
        }

        private static bool IsEmailValid(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
    }
}
