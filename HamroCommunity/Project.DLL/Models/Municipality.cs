using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public sealed class Municipality : CustomEntity
    {
        public Municipality(int Id, string municipalityNameInNepali, string municipalityNameInEnglish, int districtId) : base(Id)
        {
            MunicipalityNameInEnglish = municipalityNameInEnglish;
            MunicipalityNameInNepali = municipalityNameInNepali;
            DistrictId = districtId;
        }

        public string MunicipalityNameInNepali { get; set; }
        public string MunicipalityNameInEnglish { get;set; }
        public int DistrictId { get; set;}
        public District? District { get; set;}
    }
}
