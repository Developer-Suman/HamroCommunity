using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class Citizenship
    {
        public string Id { get; set; }
        public string? IssuedDate { get; set; }
        public string? IssuedDistrict { get; set; }
        public string? VDCOrMunicipality { get; set; }
        public string? WardNumber { get; set; }
        public string? DOB { get; set; }
        public string? CitizenshipNumber { get; set; }

        //ForeignKey
        public string DocumentsImagesId { get; set; }
        public DocumentImages DocumentImages { get; set; }
        public string DocumentsId { get;set; }
        public Documents Documents { get; set; }
    }
}
