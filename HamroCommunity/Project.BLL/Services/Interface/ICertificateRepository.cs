using Microsoft.AspNetCore.Http;
using Project.BLL.DTOs.CertificateDTOs;
using Project.BLL.DTOs.Pagination;
using Project.DLL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface ICertificateRepository
    {
        Task<Result<CertificateGetDTOs>> SaveCertificate(CertificateCreateDTOs certificateCreateDTOs, List<IFormFile> certificateFiles);
        Task<Result<CertificateGetDTOs>> GetById(string CertificateId, CancellationToken cancellationToken);
        Task<Result<CertificateGetDTOs>> DeleteCertificate(string certificateId, CancellationToken cancellationToken);
        Task<Result<CertificateGetDTOs>> UpdateCertificate(string CertificateId,CertificateUpdateDTOs certificateUpdateDTOs, List<IFormFile> multipleFiles);
        Task<Result<PagedResult<CertificateGetDTOs>>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken);

    }
}
