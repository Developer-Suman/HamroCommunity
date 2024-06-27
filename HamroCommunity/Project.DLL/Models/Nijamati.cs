using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class Nijamati : Entity
    {
        public Nijamati(): base(null)
        {
            
        }
        public Nijamati(
            string id,
            string nijamatiName
            ): base(id)
        {
            NijamatiName = nijamatiName;
        }
        public string NijamatiName { get;set; }
        //NavigationProperty

        public string DepartmentId { get;set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
        public string DocumentsId { get; set; }
        [ForeignKey("DocumentsId")]
        public Documents Documents { get; set; }
    }
}
