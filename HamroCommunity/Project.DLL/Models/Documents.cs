using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class Documents : Entity
    {
        public Documents(): base(null) { }
    
        public Documents(
            string Id,
            string documentType,
            string createdAt,
            string updatedBy,
            string signitureId,
            string citizenshipId,
            string userDataId
            ) : base(Id)
        {

            DocumentType = documentType;
            CreatedAt = createdAt;
            UpdatedBy = updatedBy;
            SignitureId = signitureId;
            CitizenshipId = citizenshipId;
            UserDataId = userDataId;
            certificateDocuments = new List<CertificateDocuments>();
        }
        public string DocumentType { get; set;}
        public string CreatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public string? SignitureId { get; set; }
        public string? CitizenshipId { get; set; }

        public string? UserDataId { get; set; }

        //NavigationProperty
        [ForeignKey("UserDataId")]

        public virtual UserData UserDatas { get; set; }
        [ForeignKey("CitizenshipId")]
        public Citizenship Citizenship { get; set; }
        [ForeignKey("SignitureId")]

        public Signature Signature { get; set; }

        public ICollection<Nijamati> Nijamatis { get; set; }
 
        public ICollection<CertificateDocuments> certificateDocuments { get; set; }

    }
}
