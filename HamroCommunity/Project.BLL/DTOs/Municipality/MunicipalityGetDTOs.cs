﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Municipality
{
    public record MunicipalityGetDTOs(int Id,
        string municipalityNameInNepali,
        string municipalityNameInEnglish,
        int districtId);
}
