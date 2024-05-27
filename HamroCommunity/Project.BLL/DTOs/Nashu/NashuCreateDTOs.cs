using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Nashu
{
    public record NashuCreateDTOs(
        string FirstName,
        string MiddleName,
        string LastName,
        string PermanentAddress,
        string TemporaryAddress,
        IFormFile SignatureImage);
  
}
