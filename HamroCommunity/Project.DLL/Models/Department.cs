using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class Department
    {
        [Key]
        public string? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }  

        //Navigation Property
        public ICollection<UserDepartment>? UserDepartments { get; set; }

    }
}
