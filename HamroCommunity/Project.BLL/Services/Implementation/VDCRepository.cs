using AutoMapper;
using Project.BLL.DTOs.Municipality;
using Project.BLL.DTOs.Vdc;
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

namespace Project.BLL.Services.Implementation
{
    public class VDCRepository : IVDCRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IMapper _mapper;

        public VDCRepository(IUnitOfWork unitOfWork, IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _memoryCacheRepository = memoryCacheRepository;
            _mapper = mapper;
            
        }
        public async Task<Result<List<VdcGetDTOs>>> GetAll(CancellationToken cancellationToken = default)
        {
            try
            {
                var cKey = CacheKeys.Vdc;
                var cacheData = await _memoryCacheRepository.GetCacheKey<List<VdcGetDTOs>>(cKey);
                if (cacheData is not null && cacheData.Count() > 0)
                {
                    return Result<List<VdcGetDTOs>>.Success(cacheData);
                }
                var VdcData = await _unitOfWork.Repository<VDC>()
                    .GetFilterAndOrderByAsync(
                    predicate: null,
                    orderby: q => q.OrderBy(p => p.Id)
                           );
                if (VdcData is null && VdcData.Count() < 0)
                {
                    return Result<List<VdcGetDTOs>>.Failure("NotFound", "VDC data are not Found");
                }
                var vdcDTOs = _mapper.Map<List<VdcGetDTOs>>(VdcData);
                await _memoryCacheRepository.SetAsync(cKey, vdcDTOs, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);



                return Result<List<VdcGetDTOs>>.Success(vdcDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching all vdc data", ex);
            }
        }

        public async Task<Result<List<VdcGetDTOs>>> GetByDistrictId(int districtId, CancellationToken cancellationToken = default)
        {
            try
            {
                var cacheKey = $"GetByDistrictId{districtId}";
                var cacheData = await _memoryCacheRepository.GetCacheKey<List<VdcGetDTOs>>(cacheKey);
                if (cacheData is not null)
                {
                    return Result<List<VdcGetDTOs>>.Success(cacheData);
                }
                int DistrictId = Convert.ToInt32(districtId);
                var VdcData = await _unitOfWork.Repository<VDC>().GetConditonalAsync(x => x.DistrictId == DistrictId);
                if (VdcData is null && VdcData.Count() < 0)
                {
                    return Result<List<VdcGetDTOs>>.Failure("NotFound", "VDC Data are not Found");
                }
                var districtDTOs = _mapper.Map<List<VdcGetDTOs>>(VdcData);

                await _memoryCacheRepository.SetAsync(cacheKey, districtDTOs, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);

                return Result<List<VdcGetDTOs>>.Success(districtDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while getting VDC by District Id", ex);
            }
        }

        public async Task<Result<VdcGetDTOs>> GetById(int VdcId, CancellationToken cancellationToken = default)
        {
            try
            {
                var cacheKey = $"GetById{VdcId}";
                var cacheData = await _memoryCacheRepository.GetCacheKey<VdcGetDTOs>(cacheKey);
                if (cacheData is not null)
                {
                    return Result<VdcGetDTOs>.Success(cacheData);
                }
                var VdcData = await _unitOfWork.Repository<VDC>().GetById(VdcId);
                if (VdcData is null)
                {
                    return Result<VdcGetDTOs>.Failure("NotFound", "VDC data are ot Found");
                }

                var VdcDTOs = _mapper.Map<VdcGetDTOs>(VdcData);

                await _memoryCacheRepository.SetAsync(cacheKey, VdcDTOs, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);

                return Result<VdcGetDTOs>.Success(VdcDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while getting VDC Details", ex);
            }
        }
    }
}
