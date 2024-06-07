using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Signiture
{
    public record SignitureUpdateDTOs(
        string SignitureId,
        string SignatureURL,
        string DocumentsId
        );
    
}
