using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Province
{
    public record ProvinceGetDTOs(int ProvinceId,
        string ProvinceNameInEnglish,
        string ProvinceNameInNepali);
   
}
