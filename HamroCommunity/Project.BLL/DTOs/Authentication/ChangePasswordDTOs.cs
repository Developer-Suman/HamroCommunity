using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Authentication
{
    public class ChangePasswordDTOs
    {
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
