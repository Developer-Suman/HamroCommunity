using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class Certificate
    {
        public string? Id { get; set; }
        public string? Grade { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Type { get; set; }
        public string? Board { get; set; }
 
        //Navigation Property
        public ICollection<CertificateImages> CertificateImages { get; set; }
        public ICollection<CertificateDocuments> CertificateDocuments { get; set; }

    }
}
