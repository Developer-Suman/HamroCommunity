﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.DocumentsDTOs
{
    public record DocumentsCreateDTOs(string DocumentType, string UpdatedBy, string SignitureId, string CitizenshipId, List<string> certificateIds );
   
}
