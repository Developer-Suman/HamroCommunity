using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Department
{
    public record DepartmentUpdateDTOs(
        string Id,
        string DepartmentNameInNepali,
        string DepartmentNameInEnglish,
        string BranchId
        );

}
