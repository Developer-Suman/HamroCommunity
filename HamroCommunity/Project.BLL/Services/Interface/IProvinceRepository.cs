using Project.DLL.Abstraction;
using Project.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface IProvinceRepository
    {
        Task<Result<Province>>
    }
}
