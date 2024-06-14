using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class Documents
    {
        public string Id { get; set; }
        public string DocumentType { get; set;}
        public string CreatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public string SignitureId { get; set; }

        //NavigationProperty

        public Signature Signature { get; set; }
 
        public ICollection<CertificateDocuments> certificateDocuments { get; set; }

    }
}
