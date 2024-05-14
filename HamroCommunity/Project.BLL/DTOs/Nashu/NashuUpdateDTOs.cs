using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Nashu
{
    public class NashuUpdateDTOs
    {
        public string NashuId { get; set; }
        public string NashuName { get; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string PermanentAddress { get; set; }
        public string TemporaryAddress { get; set; }
    }
}
