using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
            string address,
            string userId,
            string imageUrl
            ): base(id)
        {
            FatherName = fatherName;
            MotherName = motherName;
            GrandFatherName = grandFatherName;
            GrandMotherName = grandMotherName;
            Address = address;
            ImageURL = imageUrl;
            UserId = userId;
            Documents = new List<Documents>();

        }

        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string GrandFatherName { get;set; }
        public string GrandMotherName { get; set;}
        public string Address { get; set; } 
        public string ImageURL { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUsers ApplicationUser { get; set; }

        public ICollection<Documents> Documents { get; set; }


    }
}
