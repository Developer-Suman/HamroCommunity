using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class CertificateDocuments : Entity
    {
        public CertificateDocuments(
            string Id,
            string documentsId,
            string certificateId
            ) : base(Id)
        {
            documentsId = DocumentsId;
            certificateId = CertificateId;
            
            
        }
        public string DocumentsId { get; set; }
        public Documents Documents { get; set; }
        public string CertificateId { get; set; }
        public Certificate Certificate { get; set; }

    }
}
