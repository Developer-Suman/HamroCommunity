using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class Branch : Entity
    {
        public Branch(): base(null) { }

        public Branch(
            string id,
            string branchNameInEnglish,
            string branchNameInNepali,
            string branchHeadNameInEnglish,
            string branchHeadNameInNepali,
            bool isActive

            ): base(id)
        {
            BranchNameInNepali = branchHeadNameInEnglish;
            BranchNameInEnglish = branchNameInEnglish;
            BranchHeadNameInEnglish = branchHeadNameInNepali;
            BranchHeadNameInNepali = branchHeadNameInNepali;
            IsActive = isActive;
            Departments = new List<Department>();
        }

        public string BranchNameInEnglish { get; set; }
        public string BranchNameInNepali { get; set; }
        public string BranchHeadNameInEnglish { get;set; }
        public string BranchHeadNameInNepali { get; set;}
        public bool IsActive { get; set; }
        public ICollection<Department> Departments { get; set; }

    }
}
