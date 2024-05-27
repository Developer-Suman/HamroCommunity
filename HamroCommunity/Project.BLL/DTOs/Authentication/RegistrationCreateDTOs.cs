using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Authentication
{
    public record RegistrationCreateDTOs(
        string Username,
        string Email,
        string Password,
        string Role
        );
    //{

        //public string? DepartmentId { get; set; }
        //public string? ProfilePictureName { get; set; }
        //public string? ProfilePictureUrl { get; set; }
        //public bool IsActive { get; set; }
    //}
}
