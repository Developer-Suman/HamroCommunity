using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public sealed class Province : CustomEntity
    {
        public Province(int Id, string provinceNameInNepali, string provinceNameInEnglish) : base(Id)
        {
            ProvinceNameInEnglish = provinceNameInEnglish;
            ProvinceNameInEnglish = provinceNameInEnglish;
        }

        public string ProvinceNameInNepali { get; set; }
        public string ProvinceNameInEnglish { get; set; }
    }
}
