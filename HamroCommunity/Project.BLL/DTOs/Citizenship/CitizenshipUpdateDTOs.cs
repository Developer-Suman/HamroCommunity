using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Citizenship
{
    public record CitizenshipUpdateDTOs(
        string Id,
        string IssuedDate,
        string IssuedDistrict,
        string VDCOrMunicipality,
        string WardNumber,
        string DOB,
        string CitizenshipNumber,
        string DocumentsId,
        List<IFormFile>? filesList
        );

}
