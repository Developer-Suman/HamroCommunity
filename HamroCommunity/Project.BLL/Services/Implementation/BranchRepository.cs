using AutoMapper;
using Microsoft.AspNetCore.Http;
using Project.BLL.DTOs.Branch;
using Project.BLL.DTOs.CertificateDTOs;
using Project.BLL.DTOs.Nashu;
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
    public class BranchRepository : IBranchRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IHelpherMethods _helpherMethods;
        private readonly IimageRepository _iimageRepository;

        public BranchRepository(IimageRepository iimageRepository, IHelpherMethods helpherMethods, ApplicationDbContext applicationDbContext, IUnitOfWork unitOfWork, IMapper mapper, IMemoryCacheRepository memoryCacheRepository)
        {
            _iimageRepository = iimageRepository;
            _context = applicationDbContext;
            _helpherMethods = helpherMethods;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCacheRepository = memoryCacheRepository;

        }
        public async Task<Result<BranchGetDTOs>> DeleteBranch(string BranchId, CancellationToken cancellationToken)
        {
            try
            {
                await _memoryCacheRepository.RemoveAsync(CacheKeys.Branch);
                var branch = await _unitOfWork.Repository<Branch>().GetByIdAsync(BranchId);
                if (branch is null)
                {
                    return Result<BranchGetDTOs>.Failure("NotFound", "Branch cannot be Found");

                }

                _unitOfWork.Repository<Branch>().Delete(branch);
                await _unitOfWork.SaveChangesAsync();
                return Result<BranchGetDTOs>.Success(_mapper.Map<BranchGetDTOs>(branch));

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Deleting");
            }
        }

        public async Task<Result<PagedResult<BranchGetDTOs>>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKeys = CacheKeys.Branch;
                var cacheData = await _memoryCacheRepository.GetCacheKey<PagedResult<BranchGetDTOs>>(cacheKeys);
                if (cacheData is not null)
                {
                    return Result<PagedResult<BranchGetDTOs>>.Success(cacheData);
                }
                var branchData = await _unitOfWork.Repository<Branch>().GetAllAsyncWithPagination();
                var branchPagedResult = await branchData.AsNoTracking().ToPagedResultAsync(pageIndex, pageSize);

                if (branchPagedResult.Data.Items is null && branchData.Any())
                {
                    return Result<PagedResult<BranchGetDTOs>>.Failure("NotFound", "Branch Data are not Found");

                }

                var branchDataDTOs = _mapper.Map<PagedResult<BranchGetDTOs>>(branchPagedResult.Data);

                await _memoryCacheRepository.SetAsync(cacheKeys, branchPagedResult.Data.Items, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);

                return Result<PagedResult<BranchGetDTOs>>.Success(branchDataDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Fetching all the data");
            }
        }

        public async Task<Result<BranchGetDTOs>> GetById(string BranchId, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = $"GetById{BranchId}";
                var cacheData = await _memoryCacheRepository.GetCacheKey<BranchGetDTOs>(cacheKey);
                if (cacheData is not null)
                {
                    return Result<BranchGetDTOs>.Success(cacheData);
                }
                var branchData = await _unitOfWork.Repository<Branch>().GetByIdAsync(BranchId);
                if (branchData is null)
                {
                    return Result<BranchGetDTOs>.Failure("NotFound", "Branch Data are not Found");
                }
                await _memoryCacheRepository.SetAsync(cacheKey, branchData, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {

                }, cancellationToken);
                return Result<BranchGetDTOs>.Success(_mapper.Map<BranchGetDTOs>(branchData));

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while getting Certificate by Id");
            }
        }

        public async Task<Result<BranchGetDTOs>> SaveBranch(BranchCreateDTOs branchCreateDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _memoryCacheRepository.RemoveAsync(CacheKeys.Branch);
                    string newId = Guid.NewGuid().ToString();
                    var branchData = new Branch(
                        newId,
                        branchCreateDTOs.branchNameInNepali,
                        branchCreateDTOs.branchNameInEnglish,
                        branchCreateDTOs.branchHeadNameInEnglish,
                        branchCreateDTOs.branchHeadNameInNepali,
                        branchCreateDTOs.isActive
                   
                        );

                    await _unitOfWork.Repository<Branch>().AddAsync(branchData);
                    await _unitOfWork.SaveChangesAsync();
                    var resultDTOs = new BranchGetDTOs(
                        branchData.Id,
                        branchData.BranchNameInNepali,
                        branchData.BranchNameInEnglish,
                        branchData.BranchHeadNameInEnglish,
                        branchData.BranchHeadNameInNepali,
                        branchData.IsActive

                        );
                    scope.Complete();

                    return Result<BranchGetDTOs>.Success(_mapper.Map<BranchGetDTOs>(branchData));

                }
                catch (Exception ex)
                {
                    scope.Dispose();

                    throw new Exception("An exception occured while Adding Nashu Data");
                }

            }
        }

        public async Task<Result<BranchGetDTOs>> UpdateBranch(string BranchId, BranchUpdateDTOs branchUpdateDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _memoryCacheRepository.RemoveAsync(CacheKeys.Branch);
                    if (string.IsNullOrEmpty(BranchId))
                    {
                        return Result<BranchGetDTOs>.Failure("Please provide a valid BranchId");
                    }
                    var branchDataToBeUpdated = await _unitOfWork.Repository<Branch>().GetByIdAsync(BranchId);
                    if (branchDataToBeUpdated is null)
                    {
                        return Result<BranchGetDTOs>.Failure("NotFound", "BranchData are not Found");
                    }

                    //Bulk Update
                    _mapper.Map(branchUpdateDTOs, branchDataToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();

                    var resultDTOs = new BranchGetDTOs(
                        branchDataToBeUpdated.Id,
                        branchDataToBeUpdated.BranchNameInNepali,
                        branchDataToBeUpdated.BranchNameInEnglish,
                        branchDataToBeUpdated.BranchHeadNameInEnglish,
                        branchDataToBeUpdated.BranchHeadNameInNepali,
                        branchDataToBeUpdated.IsActive
                        );
                    scope.Complete();

                    return Result<BranchGetDTOs>.Success(_mapper.Map<BranchGetDTOs>(resultDTOs));

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An exception occured while Updating Branch Data");
                }

            }
        }
    }
}
