using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class Signature : Entity
    {
        public Signature() : base(null) { }

        public Signature(
            string Id,
            string signatureURL,
            string createdAt
            ): base(Id)
        {
            SignatureURL = signatureURL;
            CreatedAt = createdAt;
            Documents = new List<Documents>();
            

        }
        public string? SignatureURL { get; set; }

        public string CreatedAt { get; set; }
        public ICollection<Documents> Documents { get; set; }
    }
}
