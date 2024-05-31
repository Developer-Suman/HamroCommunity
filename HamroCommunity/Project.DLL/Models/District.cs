using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public sealed class District : CustomEntity
    {
        public District(int Id,
            string districtNameInNepali,
            string districtNameInEnglish,
            int provinceId) : base(Id)
        {
            ProvinceId = provinceId;
            DistrictNameInEnglish = districtNameInEnglish;
            DistrictNameInNepali = districtNameInNepali;
        }

        public string DistrictNameInNepali { get; set; }
        public string DistrictNameInEnglish { get; set; }
        public int ProvinceId { get; set; }
        public Province? Province { get; set; }
    }
}
