using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.DocumentsDTOs
{
    public record DocumentsGetDTOs(
        string Id,
        string DocumentType,
        string UpdatedBy,
        string CreatedBy,
        string SignitureId,
        string CitizenshiId,
        string UserDatasId,
        List<string> certificateIds);
  
}
