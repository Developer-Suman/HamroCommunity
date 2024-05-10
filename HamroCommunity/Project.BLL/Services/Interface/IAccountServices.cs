using Project.BLL.DTOs;
using Project.DLL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface IAccountServices
    {
        Task<Result<RegistrationCreateDTOs>> RegisterUser (RegistrationCreateDTOs userModel);
        Task<Result<TokenDTOs>> LoginUser (LogInDTOs logInDTOs);
    }
}
