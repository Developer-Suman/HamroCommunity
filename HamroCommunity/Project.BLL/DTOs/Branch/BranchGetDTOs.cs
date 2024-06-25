using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Branch
{
    public record BranchGetDTOs(
        string Id,
        string branchNameInNepali,
        string branchNameInEnglish,
        string branchHeadNameInEnglish,
        string branchHeadNameInNepali,
        bool isActive
        );
   
}
