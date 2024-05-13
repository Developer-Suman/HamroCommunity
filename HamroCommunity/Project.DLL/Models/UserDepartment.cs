using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class UserDepartment
    {
        [Key]
        public string? UserDepartmentId { get; set; }
        public string? UserId { get; set; }
        public ApplicationUsers? User { get; set; }
        public string? DepartentId { get; set; }
        public Department? Department { get; set; }
    }
}
