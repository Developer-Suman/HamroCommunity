using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class UserData : Entity
    {
        public UserData() : base(null) { }

        public UserData(
            string id,
            string fatherName,
            string motherName,
            string grandFatherName,
            string grandMotherName,
            string address
            ): base(id)
        {
            FatherName = fatherName;
            MotherName = motherName;
            GrandFatherName = grandFatherName;
            GrandMotherName = grandMotherName;
            Address = address;
    
        }

        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string GrandFatherName { get;set; }
        public string GrandMotherName { get; set;}
        public string Address { get; set; } 
        public string ImageURL { get; set; }


    }
}
