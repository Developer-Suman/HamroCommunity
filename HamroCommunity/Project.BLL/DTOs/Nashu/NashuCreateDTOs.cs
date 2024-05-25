﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.DTOs.Nashu
{
    public class NashuCreateDTOs
    {
        public string FirstName { get;set; }
        public string? MiddleName { get; set; }
        public string LastName { get;set; }
        public string PermanentAddress { get;set; }
        public string TemporaryAddress { get; set; }
        public IFormFile SignatureImage { get; set; }
    }
}
