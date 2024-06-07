using Project.BLL.DTOs.DocumentsDTOs;
using Project.DLL.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Interface
{
    public interface IDocumentsRepository
    {
        Task<Result<DocumentsGetDTOs>> SaveDocuments(DocumentsCreateDTOs documentsCreateDTOs);
        Task<Result<DocumentsGetDTOs>> GetById(string DocumentsId, CancellationToken cancellationToken);
        Task<Result<DocumentsGetDTOs>> DeleteDocuments(string DocumentsId); 
        Task<Result<DocumentsGetDTOs>> UpdateDocuments(string DocumentsId, DocumentsUpdateDTOs documentsUpdateDTOs);
        Task<Result<List<DocumentsGetDTOs>>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken);
    }
}
