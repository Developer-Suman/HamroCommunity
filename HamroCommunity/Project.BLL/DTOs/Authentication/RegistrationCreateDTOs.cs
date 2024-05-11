using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Authentication
{
    public class RegistrationCreateDTOs
    {
        public string Username { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Email is Required")]
        public string Password { get; set; }
        public string Role { get; set; }
        //public string? DepartmentId { get; set; }
        //public string? ProfilePictureName { get; set; }
        //public string? ProfilePictureUrl { get; set; }
        //public bool IsActive { get; set; }
    }
}
