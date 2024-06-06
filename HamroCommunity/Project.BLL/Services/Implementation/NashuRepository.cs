using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.BLL.DTOs.Nashu;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using Project.DLL.DbContext;
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
    public class NashuRepository : INashuRepository
    {

        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _cacheRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IimageRepository _iimageRepository;
        private readonly IHelpherMethods _helpherMethods;
        private readonly ApplicationDbContext _applicationDbContext;


        public NashuRepository(ApplicationDbContext applicationDbContext ,IMapper mapper, IMemoryCacheRepository memoryCacheRepository,IHelpherMethods helpherMethods, IUnitOfWork unitOfWork,IimageRepository iimageRepository)
        {
            _iimageRepository = iimageRepository;
            _mapper = mapper;
            _cacheRepository = memoryCacheRepository;
            _unitOfWork = unitOfWork;
            _helpherMethods = helpherMethods;
            _applicationDbContext = applicationDbContext;
            
        }
        public async Task<Result<NashuGetDTOs>> DeleteNashuData(string NashuId)
        {
            try
            {
                await _cacheRepository.RemoveAsync(CacheKeys.Nashu);
                var nashuData = await _unitOfWork.Repository<Nashu>().GetByIdAsync(NashuId);
                if(nashuData is null)
                {
                    return Result<NashuGetDTOs>.Failure("NotFound", "Nashu Data Not found to be deleted");
                }
                _unitOfWork.Repository<Nashu>().Delete(nashuData);
                await _unitOfWork.SaveChangesAsync();
                return Result<NashuGetDTOs>.Success(_mapper.Map<NashuGetDTOs>(nashuData));

            }catch(Exception ex)
            {
                throw new Exception("An error occured while deleting");
            }
        }


        public async Task<Result<List<NashuGetDTOs>>> GetAllNashuData(int page, int pageSize, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKeys = CacheKeys.Nashu;
                var cacheData = await _cacheRepository.GetCacheKey<List<NashuGetDTOs>>(cacheKeys);


                //DateTime date = DateTime.Now;
                //DateTime specificDate = new DateTime(1998, 5, 30);
                //var exectDate = _helpherMethods.CalculateAge(specificDate, date);




                if(cacheData is not null && cacheData.Count > 0)
                {
                    return Result<List<NashuGetDTOs>>.Success(cacheData);
                }
 

                var nashuQuery = await _unitOfWork.Repository<Nashu>().GetAllAsyncWithPagination();
                var nashuPagedResult = await nashuQuery.ToPagedResultAsync(page, pageSize);


                if (nashuQuery is null && !nashuQuery.Any())
                {
                    return Result<List<NashuGetDTOs>>.Failure("NoContent", "Nashu Data are not found");
                };
                var nashuDataDTOs = _mapper.Map<List<NashuGetDTOs>>(nashuQuery);
                await _cacheRepository.SetAsync(cacheKeys, nashuDataDTOs, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken) ;

                return Result<List<NashuGetDTOs>>.Success(nashuDataDTOs);

            }catch(Exception ex)
            {
                throw new Exception("An error occured while Fetching all Data");
            }
        }

        public async Task<Result<NashuGetDTOs>> GetNashuDataById(string NashuId, CancellationToken cancellationToken)
        {
          
            try
            {
                var cacheKeys = $"GetNashuDataById{NashuId}";
                var cacheData = await _cacheRepository.GetCacheKey<NashuGetDTOs>(cacheKeys);

                if (cacheData is not null)
                {
                    return Result<NashuGetDTOs>.Success(cacheData);
                }
                if(NashuId is null)
                {
                    return Result<NashuGetDTOs>.Failure("Please provide NashuId");

                }
                //var nashuData = await _unitOfWork.Repository<Nashu>().GetConditonalAsync(x=>x.NashuId == NashuId);
                var nashuData = await _unitOfWork.Repository<Nashu>().GetByIdAsync(NashuId);

                await _cacheRepository.SetAsync(cacheKeys, nashuData, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);
           
                return Result<NashuGetDTOs>.Success(_mapper.Map<NashuGetDTOs>(nashuData));

            }
            catch(Exception ex)
            {
                throw new Exception("An error occured while fetching nashu data");
            }
            
        }



        public async Task<Result<NashuGetDTOs>> SaveNashuData(NashuCreateDTOs nashuCreateDTOs)
        {
            using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _cacheRepository.RemoveAsync(CacheKeys.Nashu);
                    var nashuData = _mapper.Map<Nashu>(nashuCreateDTOs);
                    if (nashuData is null)
                    {
                        return Result<NashuGetDTOs>.Failure("Error occured while mapping Entity");
                    }

                    string imagesUrl = await _iimageRepository.AddSingle(nashuCreateDTOs.SignatureImage);
                    if(imagesUrl is null)
                    {
                        return Result<NashuGetDTOs>.Failure("Image cannot be saved");
                    }
           

                    nashuData.NashuId = Guid.NewGuid().ToString();
                    nashuData.SignatureImageUrl = imagesUrl;
                    await _unitOfWork.Repository<Nashu>().AddAsync(nashuData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    return Result<NashuGetDTOs>.Success(_mapper.Map<NashuGetDTOs>(nashuData));

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                   
                    throw new Exception("An exception occured while Adding Nashu Data");
                }

            }
           
        }

        public async Task<Result<NashuGetDTOs>> UpdateNashuData([FromRoute] string NashuId, NashuUpdateDTOs nashuUpdateDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _cacheRepository.RemoveAsync(CacheKeys.Nashu);
                    if(string.IsNullOrEmpty(NashuId))
                    {
                        return Result<NashuGetDTOs>.Failure("Please provide a valid NashuId");
                    }
                    var nashuDataToBeUpdated = await _unitOfWork.Repository<Nashu>().GetByIdAsync(NashuId);
                    if(nashuDataToBeUpdated is null)
                    {
                        return Result<NashuGetDTOs>.Failure("NotFound", "NashuData are not Found");
                    }

                    //Bulk Update
                    _mapper.Map(nashuUpdateDTOs, nashuDataToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();
                    return Result<NashuGetDTOs>.Success(_mapper.Map<NashuGetDTOs>(nashuUpdateDTOs));

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An exception occured while Updating Nashu Data");
                }

            }
          
        }
    }
}
