using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public sealed class Vdc : CustomEntity
    {
        public Vdc(int Id, string vdcNameInEnglish, string vdcNameInNepali, int districtId): base(Id)
        {
            VDCNaemInEnglish = vdcNameInEnglish;
            VDCNameInNepali = vdcNameInNepali;
            DistrictId = districtId;
            
        }
        public string VDCNaemInEnglish { get; set; }
        public string VDCNameInNepali { get; set; }
        public int DistrictId { get; set;}
        public District? District { get; set;}
    }
}
