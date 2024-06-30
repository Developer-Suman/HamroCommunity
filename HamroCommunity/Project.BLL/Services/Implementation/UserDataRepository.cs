using AutoMapper;
using Microsoft.AspNetCore.Http;
using Project.BLL.DTOs.CertificateDTOs;
using Project.BLL.DTOs.Pagination;
using Project.BLL.DTOs.UserData;
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
    public class UserDataRepository : IUserDataRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IHelpherMethods _helpherMethods;
        private readonly IimageRepository _iimageRepository;

        public UserDataRepository(IimageRepository iimageRepository, IHelpherMethods helpherMethods, ApplicationDbContext applicationDbContext, IUnitOfWork unitOfWork, IMapper mapper, IMemoryCacheRepository memoryCacheRepository)
        {
            _iimageRepository = iimageRepository;
            _context = applicationDbContext;
            _helpherMethods = helpherMethods;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCacheRepository = memoryCacheRepository;

        }
        public async Task<Result<GetUserDataDTOs>> DeleteUserData(string UserDataId)
        {
            try
            {
                await _memoryCacheRepository.RemoveAsync(CacheKeys.UserData);
                var userdata = await _unitOfWork.Repository<UserData>().GetByIdAsync(UserDataId);
                if (userdata is null)
                {
                    return Result<GetUserDataDTOs>.Failure("NotFound", "UserData cannot be Found");

                }

                _unitOfWork.Repository<UserData>().Delete(userdata);
                await _unitOfWork.SaveChangesAsync();
                return Result<GetUserDataDTOs>.Success(_mapper.Map<GetUserDataDTOs>(userdata));

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Deleting");
            }
        }

        public async Task<Result<PagedResult<GetUserDataDTOs>>> GetAllUserData(int page, int pageSize, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKeys = CacheKeys.UserData;
                var cacheData = await _memoryCacheRepository.GetCacheKey<PagedResult<GetUserDataDTOs>>(cacheKeys);
                if (cacheData is not null)
                {
                    return Result<PagedResult<GetUserDataDTOs>>.Success(cacheData);
                }

                //var imgs = await _context.certificates.Include(x => x.CertificateImages).ToListAsync();
                var userData = await _unitOfWork.Repository<UserData>().GetAllAsyncWithPagination();
                var userDataPagedResult = await userData
                                        .Select(x => new GetUserDataDTOs(
                                            x.Id,
                                            x.FatherName,
                                            x.MotherName,
                                            x.GrandFatherName,  
                                            x.GrandMotherName,
                                            x.Address,
                                            x.UserId,
                                            x.ImageURL
                                        ))
                                        .ToPagedResultAsync(page, pageSize);


                if (userDataPagedResult.Data.Items is null && userData.Any())
                {
                    return Result<PagedResult<GetUserDataDTOs>>.Failure("NotFound", "UserData are not Found");

                }

                var userDataDTOs = _mapper.Map<PagedResult<GetUserDataDTOs>>(userDataPagedResult.Data);


                await _memoryCacheRepository.SetAsync(cacheKeys, userData, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);

                return Result<PagedResult<GetUserDataDTOs>>.Success(userDataDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Fetching all the data");
            }
        }

        public async Task<Result<GetUserDataDTOs>> GetUserDataById(string UserDataId, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = $"GetById{UserDataId}";
                var cacheData = await _memoryCacheRepository.GetCacheKey<GetUserDataDTOs>(cacheKey);
                if (cacheData is not null)
                {
                    return Result<GetUserDataDTOs>.Success(cacheData);
                }
                var userData = await _unitOfWork.Repository<UserData>().GetByIdAsync(UserDataId);
                if (userData is null)
                {
                    return Result<GetUserDataDTOs>.Failure("NotFound", "User Data are not Found");
                }
                await _memoryCacheRepository.SetAsync(cacheKey, userData, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {

                }, cancellationToken);

                var resultDTO = new GetUserDataDTOs(
                         userData.Id,
                         userData.FatherName,
                         userData.MotherName,
                         userData.GrandFatherName,
                         userData.GrandMotherName,
                         userData.Address,
                         userData.UserId,
                         userData.ImageURL
                     );

                return Result<GetUserDataDTOs>.Success(_mapper.Map<GetUserDataDTOs>(resultDTO));

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while getting Certificate by Id");
            }
        }

        public async Task<Result<GetUserDataDTOs>> SaveUserData(CreateUserDataDTOs createUserDataDTOs, IFormFile imageUrl, string UserId)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        await _memoryCacheRepository.RemoveAsync(CacheKeys.Certificate);
                        string newId = Guid.NewGuid().ToString();
                        //var certificateData = _mapper.Map<Certificate>(certificateCreateDTOs);
                        var userData = new UserData(
                            newId,
                            createUserDataDTOs.fatherName,
                            createUserDataDTOs.motherName,
                            createUserDataDTOs.grandFatherName,
                            createUserDataDTOs.grandMotherName,
                            createUserDataDTOs.address,
                            UserId,
                            await _iimageRepository.AddSingle(imageUrl)
                        );

                        if (userData == null)
                        {
                            return Result<GetUserDataDTOs>.Failure("NotFound", "Error occurred while mapping");
                        }

                        await _unitOfWork.Repository<UserData>().AddAsync(userData);
                        await _unitOfWork.SaveChangesAsync();




                        // Map the certificate data to the result DTO and include image URLs
                        var resultDTO = new GetUserDataDTOs(
                            userData.Id,
                            userData.FatherName,
                            userData.MotherName,
                            userData.GrandFatherName,
                            userData.GrandMotherName,
                            userData.Address,
                            userData.UserId,
                            userData.ImageURL
                        );

                        scope.Complete();

                        return Result<GetUserDataDTOs>.Success(_mapper.Map<GetUserDataDTOs>(resultDTO));


                    }
                    catch (Exception ex)
                    {
                        scope.Dispose();
                        throw new Exception("An error occured while Saving Data", ex);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while saving error", ex);
            }
        }

        public async Task<Result<GetUserDataDTOs>> UpdateUserData(string UserDataId, UpdateUserDataDTOs updateUserDataDTOs, IFormFile imageUrl)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    await _memoryCacheRepository.RemoveAsync(CacheKeys.UserData);
                    if (string.IsNullOrEmpty(UserDataId))
                    {
                        return Result<GetUserDataDTOs>.Failure("Please provide a valid data");
                    }
                    var userDataToBeUpdated = await _unitOfWork.Repository<UserData>().GetByIdAsync(UserDataId);
                    if (userDataToBeUpdated is null)
                    {
                        return Result<GetUserDataDTOs>.Failure("NotFound", "userData are not Found");
                    }


                    string imageUrls = _context.UserDatas
                    .Where(x => x.Id == UserDataId && !string.IsNullOrEmpty(x.ImageURL))
                    .Select(x => x.ImageURL)
                    .FirstOrDefault();

                    string  updatedImageUrl = await _iimageRepository.UpdateSingle(imageUrl, imageUrls);


                    userDataToBeUpdated.ImageURL = updatedImageUrl;

                    //Bulk Update
                    _mapper.Map(updateUserDataDTOs, userDataToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();


                    var resultDTO = new GetUserDataDTOs(
                         userDataToBeUpdated.Id,
                            userDataToBeUpdated.FatherName,
                            userDataToBeUpdated.MotherName,
                            userDataToBeUpdated.GrandFatherName,
                            userDataToBeUpdated.GrandMotherName,
                            userDataToBeUpdated.Address,
                            userDataToBeUpdated.UserId,
                            userDataToBeUpdated.ImageURL
                  );


                    scope.Complete();

                    return Result<GetUserDataDTOs>.Success(_mapper.Map<GetUserDataDTOs>(resultDTO));


                }

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while updating UserDTOs", ex);
            }
        }
    }
    
}
