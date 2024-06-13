﻿using Project.DLL.Premetives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Models
{
    public class CertificateImages : Entity
    {
        public CertificateImages(
            string Id,
            string certificateImgUrl,
            string certificateId
            ) : base(Id)
        {
            certificateImgUrl = CertificateImgURL;
            certificateId = CertificateId;
            
        }
        public string? CertificateImgURL { get; set; }
        public string? CertificateId { get; set; }
        public Certificate Certificate { get; set; }

        //NavigationProperty


    }
}
