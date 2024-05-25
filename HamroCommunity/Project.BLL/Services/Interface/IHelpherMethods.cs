using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface IHelpherMethods
    {
        bool IsImage(string contentType);
        void CompressFile(string inputFilePath, string outputFilePath);
        bool CompareImage(IFormFile imageFile1, string imagePath2);
    }
}
