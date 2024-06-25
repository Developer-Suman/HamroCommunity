using AutoMapper;
using Project.BLL.DTOs.Branch;
using Project.BLL.DTOs.Department;
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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IHelpherMethods _helpherMethods;
        private readonly IimageRepository _iimageRepository;

        public DepartmentRepository(IimageRepository iimageRepository, IHelpherMethods helpherMethods, ApplicationDbContext applicationDbContext, IUnitOfWork unitOfWork, IMapper mapper, IMemoryCacheRepository memoryCacheRepository)
        {
            _iimageRepository = iimageRepository;
            _context = applicationDbContext;
            _helpherMethods = helpherMethods;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCacheRepository = memoryCacheRepository;

        }
        public async Task<Result<DepartmentGetDTOs>> DeleteDepartment(string DepartmentId, CancellationToken cancellationToken)
        {
            try
            {
                await _memoryCacheRepository.RemoveAsync(CacheKeys.Department);
                var department = await _unitOfWork.Repository<Department>().GetByIdAsync(DepartmentId);
                if (department is null)
                {
                    return Result<DepartmentGetDTOs>.Failure("NotFound", "Department cannot be Found");

                }

                _unitOfWork.Repository<Department>().Delete(department);
                await _unitOfWork.SaveChangesAsync();
                return Result<DepartmentGetDTOs>.Success(_mapper.Map<DepartmentGetDTOs>(department));

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Deleting");
            }
        }

        public async Task<Result<PagedResult<DepartmentGetDTOs>>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKeys = CacheKeys.Branch;
                var cacheData = await _memoryCacheRepository.GetCacheKey<PagedResult<DepartmentGetDTOs>>(cacheKeys);
                if (cacheData is not null)
                {
                    return Result<PagedResult<DepartmentGetDTOs>>.Success(cacheData);
                }
                var departmentData = await _unitOfWork.Repository<Department>().GetAllAsyncWithPagination();
                var departmentPagedResult = await departmentData.AsNoTracking().ToPagedResultAsync(pageIndex, pageSize);

                if (departmentPagedResult.Data.Items is null && departmentData.Any())
                {
                    return Result<PagedResult<DepartmentGetDTOs>>.Failure("NotFound", "Department Data are not Found");

                }

                var departmentDataDTOs = _mapper.Map<PagedResult<DepartmentGetDTOs>>(departmentPagedResult.Data);

                await _memoryCacheRepository.SetAsync(cacheKeys, departmentPagedResult.Data.Items, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);

                return Result<PagedResult<DepartmentGetDTOs>>.Success(departmentDataDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Fetching all the data");
            }
        }

        public async Task<Result<DepartmentGetDTOs>> GetById(string DepartmentId, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = $"GetById{DepartmentId}";
                var cacheData = await _memoryCacheRepository.GetCacheKey<DepartmentGetDTOs>(cacheKey);
                if (cacheData is not null)
                {
                    return Result<DepartmentGetDTOs>.Success(cacheData);
                }
                var departmentData = await _unitOfWork.Repository<Department>().GetByIdAsync(DepartmentId);
                if (departmentData is null)
                {
                    return Result<DepartmentGetDTOs>.Failure("NotFound", "Department Data are not Found");
                }
                await _memoryCacheRepository.SetAsync(cacheKey, departmentData, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {

                }, cancellationToken);
                return Result<DepartmentGetDTOs>.Success(_mapper.Map<DepartmentGetDTOs>(departmentData));

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while getting Certificate by Id");
            }
        }

        public async Task<Result<DepartmentGetDTOs>> SaveDepartment(DepartmentCreateDTOs departmentCreateDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _memoryCacheRepository.RemoveAsync(CacheKeys.Department);
                    string newId = Guid.NewGuid().ToString();
                    var departmentData = new Department(
                        newId,
                        departmentCreateDTOs.DepartmentNameInNepali,
                        departmentCreateDTOs.DepartmentNameInEnglish,
                        departmentCreateDTOs.BranchId

                        );

                    await _unitOfWork.Repository<Department>().AddAsync(departmentData);
                    await _unitOfWork.SaveChangesAsync();
                    var resultDTOs = new DepartmentGetDTOs(
                        departmentData.Id,
                        departmentData.DepartmentNameInNepali,
                        departmentData.DepartmentNameInEnglish,
                        departmentData.BranchId
                        );
                    scope.Complete();

                    return Result<DepartmentGetDTOs>.Success(_mapper.Map<DepartmentGetDTOs>(resultDTOs));

                }
                catch (Exception ex)
                {
                    scope.Dispose();

                    throw new Exception("An exception occured while Adding Department Data");
                }

            }
        }

        public async Task<Result<DepartmentGetDTOs>> UpdateDepartment(string DepartmentId, DepartmentUpdateDTOs departmentUpdateDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _memoryCacheRepository.RemoveAsync(CacheKeys.Department);
                    if (string.IsNullOrEmpty(DepartmentId))
                    {
                        return Result<DepartmentGetDTOs>.Failure("Please provide a valid DepartmentId");
                    }
                    var departmentDataToBeUpdated = await _unitOfWork.Repository<Department>().GetByIdAsync(DepartmentId);
                    if (departmentDataToBeUpdated is null)
                    {
                        return Result<DepartmentGetDTOs>.Failure("NotFound", "DepartmentData are not Found");
                    }

                    //Bulk Update
                    _mapper.Map(departmentUpdateDTOs, departmentDataToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();

                    var resultDTOs = new DepartmentGetDTOs(
                        departmentDataToBeUpdated.Id,
                        departmentDataToBeUpdated.DepartmentNameInNepali,
                        departmentDataToBeUpdated.DepartmentNameInEnglish,
                        departmentDataToBeUpdated.BranchId

                        );
                    scope.Complete();

                    return Result<DepartmentGetDTOs>.Success(_mapper.Map<DepartmentGetDTOs>(resultDTOs));

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An exception occured while Updating Department Data");
                }

            }
        }
    }
}
