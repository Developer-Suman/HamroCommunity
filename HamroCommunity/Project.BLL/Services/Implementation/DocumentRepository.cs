using AutoMapper;
using Project.BLL.DTOs.DocumentsDTOs;
using Project.BLL.DTOs.Signiture;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using Project.DLL.Models;
using Project.DLL.RepoInterface;
using Project.DLL.Static.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Project.BLL.Services.Implementation
{
    public class DocumentRepository : IDocumentsRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;

        public DocumentRepository(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCacheRepository memoryCacheRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCacheRepository = memoryCacheRepository;
            
        }
        public async Task<Result<DocumentsGetDTOs>> DeleteDocuments(string DocumentsId)
        {
            try
            {
                await _memoryCacheRepository.RemoveAsync(CacheKeys.Documents);
                var documentsData = await _unitOfWork.Repository<Documents>().GetByIdAsync(DocumentsId);
                if (documentsData is null)
                {
                    return Result<DocumentsGetDTOs>.Failure("NotFound", "Document Data is not Found");

                }
                _unitOfWork.Repository<Documents>().Delete(documentsData);
                await _unitOfWork.SaveChangesAsync();
                return Result<DocumentsGetDTOs>.Success(_mapper.Map<DocumentsGetDTOs>(documentsData));

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Deleting Documents");
            }
        }

        public async Task<Result<List<DocumentsGetDTOs>>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKeys = CacheKeys.Documents;
                var cacheData = await _memoryCacheRepository.GetCacheKey<List<DocumentsGetDTOs>>(cacheKeys);
                if (cacheData is not null)
                {
                    return Result<List<DocumentsGetDTOs>>.Success(cacheData);
                }
                var documentsData = await _unitOfWork.Repository<Documents>().GetAllAsyncWithPagination();
                var documentPagedResult = await documentsData.ToPagedResultAsync(pageIndex, pageSize);

                if (documentPagedResult is null && !documentsData.Any())
                {
                    return Result<List<DocumentsGetDTOs>>.Failure("NotFound", "Documents data are not Found");

                }

                var documentsDataDTOs = _mapper.Map<List<DocumentsGetDTOs>>(documentPagedResult);

                await _memoryCacheRepository.SetAsync(cacheKeys, documentsData, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken); ;
                return Result<List<DocumentsGetDTOs>>.Success(documentsDataDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching all data");
            }
        }

        public async Task<Result<DocumentsGetDTOs>> GetById(string DocumentsId, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKeys = $"GetById{DocumentsId}";
                var cacheData = await _memoryCacheRepository.GetCacheKey<DocumentsGetDTOs>(cacheKeys);
                if (cacheData is not null)
                {
                    return Result<DocumentsGetDTOs>.Success(cacheData);
                }
                var documentsData = await _unitOfWork.Repository<Documents>().GetByIdAsync(DocumentsId);
                if (documentsData is null)
                {
                    return Result<DocumentsGetDTOs>.Failure("NotFound", "Documents data is not Found");
                }
                await _memoryCacheRepository.SetAsync(cacheKeys, documentsData, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {

                }, cancellationToken);
                return Result<DocumentsGetDTOs>.Success(_mapper.Map<DocumentsGetDTOs>(documentsData));

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching data by Id");

            }
        }

        public async Task<Result<DocumentsGetDTOs>> SaveDocuments(DocumentsCreateDTOs documentsCreateDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _memoryCacheRepository.RemoveAsync(CacheKeys.Documents);
                    var documentsData = _mapper.Map<Documents>(documentsCreateDTOs);
                    if (documentsData is null)
                    {
                        return Result<DocumentsGetDTOs>.Failure("Error occured while mapping");

                    }

                    documentsData.Id = Guid.NewGuid().ToString();
                    documentsData.UpdatedBy = "Suman";
                    documentsData.CreatedAt = DateTime.Now.ToString();
                    await _unitOfWork.Repository<Documents>().AddAsync(documentsData);
                    await _unitOfWork.SaveChangesAsync();

                    scope.Complete();

                    return Result<DocumentsGetDTOs>.Success(_mapper.Map<DocumentsGetDTOs>(documentsData));


                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occured while Saving Data");
                }
            }
        }

        public async Task<Result<DocumentsGetDTOs>> UpdateDocuments(string DocumentsId, DocumentsUpdateDTOs documentsUpdateDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _memoryCacheRepository.RemoveAsync(CacheKeys.Documents);
                    var documentsDataToBeUpdated = await _unitOfWork.Repository<Signature>().GetByIdAsync(DocumentsId);
                    if (documentsDataToBeUpdated is null)
                    {
                        return Result<DocumentsGetDTOs>.Failure("NotFound", "Documents are not Found");
                    }

                    //Builk Update 
                    _mapper.Map(documentsUpdateDTOs, documentsDataToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    return Result<DocumentsGetDTOs>.Success(_mapper.Map<DocumentsGetDTOs>(documentsUpdateDTOs));

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occured while updating");
                }
            }
        }

      
    }
}
