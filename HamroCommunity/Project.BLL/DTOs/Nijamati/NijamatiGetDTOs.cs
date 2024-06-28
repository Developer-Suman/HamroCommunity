using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Nijamati
{
    public record NijamatiGetDTOs(
        string Id,
        string NijamatName,
        string DepartmentId,
        string DocumentsId
        );
  
}
