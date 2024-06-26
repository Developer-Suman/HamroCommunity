﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class ApplicationUsers : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set;}
        public string? Address { get; set; }
        //public string? DepartmentId { get; set; }
        //public string? ProfilePictureName { get; set; }
        //public string? ProfilePictureUrl { get; set; }
        //public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
       
        //Using virtual keyword to enable lazyloading which means only needed data are fetched.
        //Naviagtaion Property
        public virtual UserData UserData { get; set; }
    }
}
