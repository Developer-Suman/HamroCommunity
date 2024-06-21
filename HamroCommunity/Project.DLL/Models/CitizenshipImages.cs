using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class CitizenshipImages : Entity
    {
        public CitizenshipImages(
            string Id,
            string imageUrl,
            DateTime createdAt,
            string citizenshipId
            ): base(Id)
        {
            CitizenshipId = citizenshipId;
            ImageUrl = imageUrl;
            CreatedAt = createdAt;
            
        }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set;}
        public string CitizenshipId { get; set; }
        [ForeignKey("CitizenshipId")]
        //NavigationProperty
        public Citizenship Citizenship { get; set; }
    }
}
