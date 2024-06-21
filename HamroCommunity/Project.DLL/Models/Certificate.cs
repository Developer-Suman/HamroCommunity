using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class Certificate : Entity
    {
        public Certificate(): base(null) { }

        public Certificate(
            string id,
            string grade,
            string type,
            string board 
            ) : base(id)
        {
            Grade = grade;
            Type = type;
            Board = board;
            CertificateImages = new List<CertificateImages>();
            CertificateDocuments = new List<CertificateDocuments>();


        }
        public string? Grade { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Type { get; set; }
        public string? Board { get; set; }

        //Navigation Property
        public ICollection<CertificateImages>? CertificateImages { get; set; } 
        public ICollection<CertificateDocuments>? CertificateDocuments { get; set; } 

    }
}
