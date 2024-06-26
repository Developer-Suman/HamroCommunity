﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface IHelpherMethods
    {

        Task<int> CalculateAge(DateTime dateOfBirth, DateTime now);
        bool IsImage(string contentType);
        void CompressFile(string inputFilePath, string outputFilePath);
        bool CompareImage(IFormFile imagePath1, string imagePath2);
    }
}
