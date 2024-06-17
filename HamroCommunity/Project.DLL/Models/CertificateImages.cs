using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class CertificateImages : Entity
    {

        public CertificateImages(
                string id,
                string certificateImgURL,
                string certificateId
                ) : base(id)
        {
            CertificateImgURL = certificateImgURL;
            CertificateId = certificateId;
        }

        public string? CertificateImgURL { get; set; }
        public string? CertificateId { get; set; }
        [ForeignKey("CertificateId")]
        public Certificate Certificate { get; set; } // Navigation property

    }
}
