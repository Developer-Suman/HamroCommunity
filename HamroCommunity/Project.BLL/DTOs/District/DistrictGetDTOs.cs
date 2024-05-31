using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.District
{
    public record DistrictGetDTOs(int districtId,
        string districtNameInNepali,
        string districtNameInEnglish,
        int provinceId);
  
}
