using AutoMapper;
using Project.BLL.DTOs.District;
using Project.BLL.DTOs.Municipality;
using Project.BLL.DTOs.Province;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using Project.DLL.Models;
using Project.DLL.RepoInterface;
using Project.DLL.Static.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project.BLL.Services.Implementation
{
    public class MunicipalityRepository : IMunicipalityRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IMapper _mapper;

        public MunicipalityRepository(IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _mapper = mapper;
            
        }
        public async Task<Result<List<MunicipalityGetDTOs>>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var cKey = CacheKeys.Municipality;
                var cacheData = await _memoryCacheRepository.GetCacheKey<List<MunicipalityGetDTOs>>(cKey);
                if (cacheData is not null && cacheData.Count() > 0)
                {
                    return Result<List<MunicipalityGetDTOs>>.Success(cacheData);
                }
                var MunicipalityData = await _unitOfWork.Repository<Municipality>()
                    .GetFilterAndOrderByAsync(
                    predicate: null,
                    orderby: q => q.OrderBy(p => p.Id)
                           );
                if (MunicipalityData is null && MunicipalityData.Count() < 0)
                {
                    return Result<List<MunicipalityGetDTOs>>.Failure("NotFound", "Municipality data are not Found");
                }
                var municipalDTOs = _mapper.Map<List<MunicipalityGetDTOs>>(MunicipalityData);
                await _memoryCacheRepository.SetAsync(cKey, municipalDTOs, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);



                return Result<List<MunicipalityGetDTOs>>.Success(municipalDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching all data", ex);
            }
        }

        public async Task<Result<List<MunicipalityGetDTOs>>> GetByDistrictId(int districtId, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = $"GetByDistrictId{districtId}";
                var cacheData = await _memoryCacheRepository.GetCacheKey<List<MunicipalityGetDTOs>>(cacheKey);
                if (cacheData is not null)
                {
                    return Result<List<MunicipalityGetDTOs>>.Success(cacheData);
                }
                int DistrictId = Convert.ToInt32(districtId);
                var municipalData = await _unitOfWork.Repository<Municipality>().GetConditonalAsync(x => x.DistrictId == DistrictId);
                if (municipalData is null && municipalData.Count() < 0)
                {
                    return Result<List<MunicipalityGetDTOs>>.Failure("NotFound", "Municipality Data are not Found");
                }
                var districtDTOs = _mapper.Map<List<MunicipalityGetDTOs>>(municipalData);

                await _memoryCacheRepository.SetAsync(cacheKey, districtDTOs, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);

                return Result<List<MunicipalityGetDTOs>>.Success(districtDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while getting Municipality by DistrictId", ex);
            }
        }

        public async Task<Result<MunicipalityGetDTOs>> GetById(int municipalityId, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = $"GetById{municipalityId}";
                var cacheData = await _memoryCacheRepository.GetCacheKey<MunicipalityGetDTOs>(cacheKey);
                if (cacheData is not null)
                {
                    return Result<MunicipalityGetDTOs>.Success(cacheData);
                }
                var municipalData = await _unitOfWork.Repository<Municipality>().GetById(municipalityId);
                if (municipalData is null)
                {
                    return Result<MunicipalityGetDTOs>.Failure("NotFound", "District data are ot Found");
                }

                var municipalDTOs = _mapper.Map<MunicipalityGetDTOs>(municipalData);

                await _memoryCacheRepository.SetAsync(cacheKey, municipalDTOs, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);

                return Result<MunicipalityGetDTOs>.Success(municipalDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while getting District Details", ex);
            }
        }
    }
}
