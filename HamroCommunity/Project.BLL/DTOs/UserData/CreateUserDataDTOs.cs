using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.UserData
{
    public record CreateUserDataDTOs(
        string fatherName,
        string motherName,
        string grandFatherName,
        string grandMotherName,
        string address
        );
   
}
