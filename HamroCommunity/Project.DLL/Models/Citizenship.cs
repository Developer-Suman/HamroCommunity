using Project.DLL.Premetives;
using System;
using System.Collections.Generic;

namespace Project.DLL.Models
{
    public class Citizenship : Entity
    {
        // Parameterless constructor for EF
        public Citizenship() : base(null) { }

        // Constructor with parameters for manual instantiation
        public Citizenship(
            string id,
            string issuedDate,
            string issuedDistrict,
            string vdcOrMunicipality,
            string wardNumber,
            string dob,
            string citizenshipNumber
        ) : base(id)
        {
            IssuedDate = issuedDate;
            IssuedDistrict = issuedDistrict;
            VdcOrMunicipality = vdcOrMunicipality;
            WardNumber = wardNumber;
            DOB = dob;
            CitizenshipNumber = citizenshipNumber;
        }

        public string? IssuedDate { get; set; }
        public string? IssuedDistrict { get; set; }
        public string? VdcOrMunicipality { get; set; }
        public string? WardNumber { get; set; }
        public string? DOB { get; set; }
        public string? CitizenshipNumber { get; set; }

        // Navigation Property
        public ICollection<CitizenshipImages> CitizenshipImages { get; set; } = new List<CitizenshipImages>();
    }
}
