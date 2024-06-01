using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Vdc
{
    public record VdcGetDTOs(int Id,
        string vdcNameInNepali,
        string vdcNameInEnglish,
        int districtId);
}
