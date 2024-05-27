using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Nashu
{
    public record NashuGetDTOs(
        string NashuId,
        string FirstName,
        string? MiddleName,
        string LastName,
        string PermanentAddress,
        string TemporaryAddress);
}
