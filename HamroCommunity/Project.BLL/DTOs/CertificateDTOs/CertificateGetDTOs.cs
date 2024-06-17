using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.CertificateDTOs
{

    public record CertificateGetDTOs(
        string Id,
        string Grade,
        string Type,
        string Board,
        List<string> certificateImages
        );
   
}
