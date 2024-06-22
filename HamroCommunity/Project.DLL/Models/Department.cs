using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class Department : Entity
    {
        public Department() : base("") { }

        public Department(
            string id,
            string departmentNameInNepali,
            string departmentNameInEnglish,
            string branchId
            ): base(id)
        {
            DepartmentNameInEnglish = departmentNameInEnglish;
            BranchId = branchId;
            DepartmentNameInNepali = departmentNameInNepali;

            
        }

        public string? DepartmentNameInNepali { get; set; }  
        public string? DepartmentNameInEnglish { get; set; }

        //Navigation Property
        public string BranchId { get; set; }
        [ForeignKey("BranchId")]
        public Branch Branch { get; set; }
        

    }
}
