using AutoMapper;
using Project.BLL.DTOs.Branch;
using Project.BLL.DTOs.Nijamati;
using Project.BLL.DTOs.Pagination;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using Project.DLL.DbContext;
using Project.DLL.Models;
using Project.DLL.RepoInterface;
using Project.DLL.Static.Cache;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Project.BLL.Services.Implementation
{
    public class NijamatiRepository : INijamatiRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IHelpherMethods _helpherMethods;
        private readonly IimageRepository _iimageRepository;


        public NijamatiRepository(IimageRepository iimageRepository, IHelpherMethods helpherMethods, ApplicationDbContext applicationDbContext, IUnitOfWork unitOfWork, IMapper mapper, IMemoryCacheRepository memoryCacheRepository)
        {
            _iimageRepository = iimageRepository;
            _context = applicationDbContext;
            _helpherMethods = helpherMethods;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCacheRepository = memoryCacheRepository;

        }
        public async Task<Result<NijamatiGetDTOs>> DeleteNijamati(string NijamatiId, CancellationToken cancellationToken)
        {
            try
            {
                await _memoryCacheRepository.RemoveAsync(CacheKeys.Nijamati);
                var njamati = await _unitOfWork.Repository<Nijamati>().GetByIdAsync(NijamatiId);
                if (njamati is null)
                {
                    return Result<NijamatiGetDTOs>.Failure("NotFound", "Nijamati cannot be Found");

                }

                _unitOfWork.Repository<Nijamati>().Delete(njamati);
                await _unitOfWork.SaveChangesAsync();
                return Result<NijamatiGetDTOs>.Success(_mapper.Map<NijamatiGetDTOs>(njamati));

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Deleting");
            }
        }

        public async Task<Result<PagedResult<NijamatiGetDTOs>>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKeys = CacheKeys.Nijamati;
                var cacheData = await _memoryCacheRepository.GetCacheKey<PagedResult<NijamatiGetDTOs>>(cacheKeys);
                if (cacheData is not null)
                {
                    return Result<PagedResult<NijamatiGetDTOs>>.Success(cacheData);
                }
                var nijamatiData = await _unitOfWork.Repository<Nijamati>().GetAllAsyncWithPagination();
                var nijamatiPagedResult = await nijamatiData.AsNoTracking().ToPagedResultAsync(pageIndex, pageSize);

                if (nijamatiPagedResult.Data.Items is null && nijamatiData.Any())
                {
                    return Result<PagedResult<NijamatiGetDTOs>>.Failure("NotFound", "Nijamati Data are not Found");

                }

                var nijamatiDataDTOs = _mapper.Map<PagedResult<NijamatiGetDTOs>>(nijamatiPagedResult.Data);

                await _memoryCacheRepository.SetAsync(cacheKeys, nijamatiPagedResult.Data.Items, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);

                return Result<PagedResult<NijamatiGetDTOs>>.Success(nijamatiDataDTOs);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Fetching all the data");
            }
        }

        public async Task<Result<NijamatiGetDTOs>> GetById(string NijamatiId, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = $"GetById{NijamatiId}";
                var cacheData = await _memoryCacheRepository.GetCacheKey<NijamatiGetDTOs>(cacheKey);
                if (cacheData is not null)
                {
                    return Result<NijamatiGetDTOs>.Success(cacheData);
                }
                var nijamatiData = await _unitOfWork.Repository<Nijamati>().GetByIdAsync(NijamatiId);
                if (nijamatiData is null)
                {
                    return Result<NijamatiGetDTOs>.Failure("NotFound", "Nijamati Data are not Found");
                }
                await _memoryCacheRepository.SetAsync(cacheKey, nijamatiData, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {

                }, cancellationToken);
                return Result<NijamatiGetDTOs>.Success(_mapper.Map<NijamatiGetDTOs>(nijamatiData));

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while getting Certificate by Id");
            }
        }

        public async Task<Result<NijamatiGetDTOs>> SaveNijamati(NijamatiCreateDTOs nijamatiCreateDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _memoryCacheRepository.RemoveAsync(CacheKeys.Nijamati);
                    string newId = Guid.NewGuid().ToString();
                    var nijamatiData = new Nijamati(
                        newId,
                        nijamatiCreateDTOs.NijamatiName,
                        nijamatiCreateDTOs.DepartmentId,
                        nijamatiCreateDTOs.DocumentsId

                        );

                    await _unitOfWork.Repository<Nijamati>().AddAsync(nijamatiData);
                    await _unitOfWork.SaveChangesAsync();
                    var resultDTOs = new NijamatiGetDTOs(
                        nijamatiData.Id,
                        nijamatiData.NijamatiName,
                        nijamatiData.DepartmentId,
                        nijamatiData.DocumentsId

                        );
                    scope.Complete();

                    return Result<NijamatiGetDTOs>.Success(_mapper.Map<NijamatiGetDTOs>(resultDTOs));

                }
                catch (Exception ex)
                {
                    scope.Dispose();

                    throw new Exception("An exception occured while Adding Nijamati Data");
                }

            }
        }

        public async Task<Result<NijamatiGetDTOs>> UpdateNijamati(string NijamatiId, NijamatiUpdateDTOs nijamatiUpdateDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _memoryCacheRepository.RemoveAsync(CacheKeys.Nijamati);
                    if (string.IsNullOrEmpty(NijamatiId))
                    {
                        return Result<NijamatiGetDTOs>.Failure("Please provide a valid NijamatiId");
                    }
                    var nijamatiDataToBeUpdated = await _unitOfWork.Repository<Nijamati>().GetByIdAsync(NijamatiId);
                    if (nijamatiDataToBeUpdated is null)
                    {
                        return Result<NijamatiGetDTOs>.Failure("NotFound", "NijamatiData are not Found");
                    }

                    //Bulk Update
                    _mapper.Map(nijamatiUpdateDTOs, nijamatiDataToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();

                    var resultDTOs = new NijamatiGetDTOs(
                        nijamatiDataToBeUpdated.Id,
                        nijamatiDataToBeUpdated.NijamatiName,
                        nijamatiDataToBeUpdated.DepartmentId,
                        nijamatiDataToBeUpdated.DocumentsId

                        );
                    scope.Complete();

                    return Result<NijamatiGetDTOs>.Success(_mapper.Map<NijamatiGetDTOs>(resultDTOs));

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An exception occured while Updating Nijamati Data");
                }

            }
        }
    }
}
