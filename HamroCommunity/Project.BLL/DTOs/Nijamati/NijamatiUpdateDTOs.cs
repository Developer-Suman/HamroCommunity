﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Nijamati
{
    public record NijamatiUpdateDTOs(
        string Id,
        string NijamatName,
        string DocumentsId
        );
 
}
