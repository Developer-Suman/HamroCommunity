using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class Signature
    {
        public string? SignatureId { get;set; }
        public string? SignatureURL { get; set; }

        public string CreatedAt { get; set; }
        public ICollection<Documents> Documents { get; set; }
    }
}
