using AutoMapper;
using Project.BLL.DTOs.District;
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

namespace Project.BLL.Services.Implementation
{
    public class DistrictRepository : IDistrictRepository
    {
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DistrictRepository(IMemoryCacheRepository memoryCacheRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _memoryCacheRepository = memoryCacheRepository;
            _unitOfWork = unitOfWork;
            
        }
        public async Task<Result<List<DistrictGetDTOs>>> GetAll(CancellationToken cancellationToken = default)
        {
            try
            {
                var cacheKey = CacheKeys.District;
                var cacheData = await _memoryCacheRepository.GetCacheKey<List<DistrictGetDTOs>>(cacheKey);
                var districtData = await _unitOfWork.Repository<District>()
                    .GetFilterAndOrderByAsync(
                    predicate: null,
                    orderby: q => q.OrderBy(p => p.Id));

                if(districtData != null && districtData.Count() < 0 )
                {
                    return Result<List<DistrictGetDTOs>>.Failure("NotFound", "District are not found");
                }

                var distrctDataDTOs = _mapper.Map<List<DistrictGetDTOs>>(districtData);
                await _memoryCacheRepository.SetAsync(cacheKey, distrctDataDTOs, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken) ;

                return Result<List<DistrictGetDTOs>>.Success(distrctDataDTOs);

            }catch (Exception ex)
            {
                throw new Exception("An error occured while fetching all the district");
            }
        }

        public async Task<Result<DistrictGetDTOs>> GetById(string DistrictId, CancellationToken cancellationToken = default)
        {
            try
            {
                var cacheKey = $"GetById{DistrictId}";
                var cacheData = await _memoryCacheRepository.GetCacheKey<DistrictGetDTOs>(cacheKey);
                var districtData = await _unitOfWork.Repository<District>().GetByIdAsync(DistrictId);
                if(districtData is null)
                {
                    return Result<DistrictGetDTOs>.Failure("NotFound", "District data are ot Found");
                }

                var districtDTOs = _mapper.Map<DistrictGetDTOs>(districtData);

                await _memoryCacheRepository.SetAsync(cacheKey, districtDTOs, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                },cancellationToken) ;

                return Result<DistrictGetDTOs>.Success(districtDTOs);

            }catch(Exception ex)
            {
                throw new Exception("An error occured while getting District Details", ex);
            }
        }

        public async Task<Result<List<DistrictGetDTOs>>> GetByProvinceId(string ProvinceId, CancellationToken cancellationToken = default)
        {
            try
            {
                var cacheKey = $"GetByProvinceId{ProvinceId}";
                var cacheData = await _memoryCacheRepository.GetCacheKey<List<DistrictGetDTOs>>(cacheKey);
                int provinceId = Convert.ToInt32(ProvinceId);
                var districtData = await _unitOfWork.Repository<District>().GetConditonalAsync(x=>x.ProvinceId == provinceId);
                if(districtData is null)
                {
                    return Result<List<DistrictGetDTOs>>.Failure("NotFound", "District Data are not Found");
                }
                var districtDTOs = _mapper.Map<List<DistrictGetDTOs>>(districtData);

                await _memoryCacheRepository.SetAsync(cacheKey, districtDTOs, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);

                return Result<List<DistrictGetDTOs>>.Success(districtDTOs);

            }catch(Exception ex)
            {
                throw new Exception("An error occured while getting District by ProvinceId", ex);
            }
        }
    }
}
