using Project.DLL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Authentication
{
    public class LogInDTOs
    {

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
