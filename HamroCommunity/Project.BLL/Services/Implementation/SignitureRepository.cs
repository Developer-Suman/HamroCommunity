using AutoMapper;
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
    public class SignitureRepository : ISignitureRepository
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IimageRepository _imageRepository;
        private readonly IHelpherMethods _helpherMethods;


        public SignitureRepository(IMapper mapper, IMemoryCacheRepository memoryCacheRepository, IUnitOfWork unitOfWork, IimageRepository iimageRepository,IHelpherMethods helpherMethods)
        {
            _mapper = mapper;
            _memoryCacheRepository = memoryCacheRepository;
            _unitOfWork = unitOfWork;
            _helpherMethods = helpherMethods;
            _imageRepository = iimageRepository;
            
        }
        public async Task<Result<SignitureGetDTOs>> DeleteSigniture(string SignitureId)
        {
            try
            {  
                await _memoryCacheRepository.RemoveAsync(CacheKeys.Signature);
                var signitureData = await _unitOfWork.Repository<Signature>().GetByIdAsync(SignitureId);
                if(signitureData is null)
                {
                    return Result<SignitureGetDTOs>.Failure("NotFound", "Signature Data is not Found");

                }
                _unitOfWork.Repository<Signature>().Delete(signitureData);
                await _unitOfWork.SaveChangesAsync();
                return Result<SignitureGetDTOs>.Success(_mapper.Map<SignitureGetDTOs>(signitureData));

            }catch(Exception ex)
            {
                throw new Exception("An error occured while Deleting Signiture");
            }
        }

        public async Task<Result<List<SignitureGetDTOs>>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKeys = CacheKeys.Signature;
                var cacheData = await _memoryCacheRepository.GetCacheKey<List<SignitureGetDTOs>>(cacheKeys);
                if(cacheData is not null)
                {
                    return Result<List<SignitureGetDTOs>>.Success(cacheData);
                }
                var signitureData = await _unitOfWork.Repository<Signature>().GetAllAsyncWithPagination();
                var signiturePagedResult = await signitureData.ToPagedResultAsync(pageIndex, pageSize);

                if( signiturePagedResult is null && !signitureData.Any())
                {
                    return Result<List<SignitureGetDTOs>>.Failure("NotFound", "Signiture data are not Found");

                }

                var signitureDataDTOs = _mapper.Map<List<SignitureGetDTOs>>(signiturePagedResult);

                await _memoryCacheRepository.SetAsync(cacheKeys, signitureDataDTOs, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);;
                return Result<List<SignitureGetDTOs>>.Success(signitureDataDTOs);

            }
            catch(Exception ex)
            {
                throw new Exception("An error occured while fetching all data");
            }
        }

        public async Task<Result<SignitureGetDTOs>> GetById(string SignitureId, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKeys = $"GetById{SignitureId}";
                var cacheData = await _memoryCacheRepository.GetCacheKey<SignitureGetDTOs>(cacheKeys);
                if(cacheData is not null)
                {
                    return Result<SignitureGetDTOs>.Success(cacheData);
                }
                var signitureData = await _unitOfWork.Repository<Signature>().GetByIdAsync(SignitureId);
                if(signitureData is null)
                {
                    return Result<SignitureGetDTOs>.Failure("NotFound", "Signiture data is not Found");
                }
                await _memoryCacheRepository.SetAsync(cacheKeys , signitureData, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {

                }, cancellationToken);
                return Result<SignitureGetDTOs>.Success(_mapper.Map<SignitureGetDTOs>(signitureData));

            }catch(Exception ex) {
                throw new Exception("An error occured while fetching data by Id");

            }
        }

        public async Task<Result<SignitureGetDTOs>> SaveSigniture(SignitureCreateDTOs signitureCreateDTOs)
        {
           using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _memoryCacheRepository.RemoveAsync(CacheKeys.Signature);
                    var signitureData = _mapper.Map<Signature>(signitureCreateDTOs);
                    if(signitureData is null)
                    {
                        return Result<SignitureGetDTOs>.Failure("Error occured while mapping");

                    }

                    string imageURL = await _imageRepository.AddSingle(signitureCreateDTOs.SignitureFile);

                    if(imageURL is null)
                    {
                        return Result<SignitureGetDTOs>.Failure("Image Url are not Created");
                    }
                    signitureData.Id = Guid.NewGuid().ToString();
                    signitureData.SignatureURL = imageURL;
              
                    signitureData.CreatedAt = DateTime.Now.ToString();
                    await _unitOfWork.Repository<Signature>().AddAsync(signitureData);
                    await _unitOfWork.SaveChangesAsync();

                    scope.Complete();

                    return Result<SignitureGetDTOs>.Success(_mapper.Map<SignitureGetDTOs>(signitureData));


                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occured while Saving Data");
                }
            }
        }

        public async Task<Result<SignitureGetDTOs>> UpdateSigniture(string SignitureId, SignitureUpdateDTOs signitureUpdateDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _memoryCacheRepository.RemoveAsync(CacheKeys.Signature);
                    var signitureDataToBeUpdated = await _unitOfWork.Repository<Signature>().GetByIdAsync(SignitureId);
                    if(signitureDataToBeUpdated is null)
                    {
                        return Result<SignitureGetDTOs>.Failure("NotFound", "Signiture are not Found");
                    }

                    //Builk Update 
                    _mapper.Map(signitureUpdateDTOs, signitureDataToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    return Result<SignitureGetDTOs>.Success(_mapper.Map<SignitureGetDTOs>(signitureUpdateDTOs));

                }catch(Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occured while updating");
                }
            }
        }
    }
}
