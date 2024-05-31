using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class VDC : CustomEntity
    {
        public VDC(int Id, string vdcNameInEnglish, string vdcNameInNepali, int districtId) : base(Id)
        {
            VdcNameInEnglish = vdcNameInEnglish;
            VdcNameInNepali = vdcNameInNepali;
            DistrictId = districtId;
            
        }
        public string VdcNameInEnglish { get; set; }
        public string VdcNameInNepali { get; set; }
        public int DistrictId { get; set;}
        public District? District { get; set;}
    }
}
