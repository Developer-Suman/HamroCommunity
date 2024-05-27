using Project.DLL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Authentication
{
    public record LogInDTOs(
        string Email,
        string Password
        );
}
