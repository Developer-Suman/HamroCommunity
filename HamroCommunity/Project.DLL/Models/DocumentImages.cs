using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class DocumentImages
    {
        public string Id { get; set;}
        public string? DocumentURL { get; set; }

        //NavigationProperty
        public ICollection<CertificatesDocumentsImages> CertificatesDocumentsImage { get; set; }
        public ICollection<Citizenship> Citizenships { get; set; }

    }
}
