using AutoMapper;
using Project.BLL.DTOs.Nashu;
using Project.BLL.DTOs.Province;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using Project.DLL.Models;
using Project.DLL.RepoInterface;
using Project.DLL.Static.Cache;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Implementation
{
    public class ProvinceRepository : IProvinceRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IMapper _mapper;

        public ProvinceRepository(IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _mapper = mapper;
            
        }

        public async Task<Result<List<ProvinceGetDTOs>>> GetAll(CancellationToken cancellationToken = default)
        {
            try
            {
                var cKey = CacheKeys.Province;
                var cacheData = await _memoryCacheRepository.GetCacheKey<List<ProvinceGetDTOs>>(cKey);
                if(cacheData is not null && cacheData.Count() > 0)
                {
                    return Result<List<ProvinceGetDTOs>>.Success(cacheData);
                }
                var ProvienceData = await _unitOfWork.Repository<Province>()
                    .GetFilterAndOrderByAsync(
                    predicate: null,
                    orderby: q => q.OrderBy(p => p.Id)
                           );
                if (ProvienceData is null && ProvienceData.Count() < 0)
                {
                    return Result<List<ProvinceGetDTOs>>.Failure("NotFound", "Province data are not Found");
                }
                var ProvinceDTOs = _mapper.Map<List<ProvinceGetDTOs>>(ProvienceData);
                await _memoryCacheRepository.SetAsync(cKey, ProvinceDTOs, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);


          
                return Result<List<ProvinceGetDTOs>>.Success(ProvinceDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching all data", ex);
            }
        }



        public async Task<Result<ProvinceGetDTOs>> GetById(int provinceId, CancellationToken cancellationToken = default)
        {
            try
            {
                var cKey = $"GetById{provinceId}";
                var cacheData =await _memoryCacheRepository.GetCacheKey<ProvinceGetDTOs>(cKey);
                if(cacheData is not null)
                {
                    return Result<ProvinceGetDTOs>.Success(cacheData);
                }

                var provinceData = await _unitOfWork.Repository<Province>().GetById(provinceId);
                if(provinceData is null)
                {
                    return Result<ProvinceGetDTOs>.Failure("NotFound", "Province Not Found");
                }

                await _memoryCacheRepository.SetAsync(cKey, provinceData, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)

                }, cancellationToken) ;

                return Result<ProvinceGetDTOs>.Success(_mapper.Map<ProvinceGetDTOs>(provinceData));

            }catch(Exception ex)
            {
                throw new Exception("An error occured while getting Province by Id", ex);
            }
        }

        
    }
}
