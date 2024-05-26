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

        //NavigationProperty

        public Citizenship Citizenship { get; set; }
        public Signature Signature { get; set; }
        public ICollection<Certificate> Certificates { get; set; }

    }
}
