using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
            DocumentsId = documentsId;
            CertificateId = certificateId;
            
            
        }
        public string DocumentsId { get; set; }
        [ForeignKey("DocumentsId")]
        public Documents Documents { get; set; }
        public string CertificateId { get; set; }
        [ForeignKey("CertificateId")]
        public Certificate Certificate { get; set; }

    }
}
