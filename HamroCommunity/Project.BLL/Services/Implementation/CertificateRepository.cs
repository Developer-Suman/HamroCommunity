using AutoMapper;
using Microsoft.AspNetCore.Http;
using Project.BLL.DTOs.CertificateDTOs;
using Project.BLL.DTOs.Citizenship;
using Project.BLL.DTOs.DocumentsDTOs;
using Project.BLL.DTOs.Pagination;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using Project.DLL.DbContext;
using Project.DLL.Models;
using Project.DLL.RepoInterface;
using Project.DLL.Static.Cache;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Project.BLL.Services.Implementation
{
    public class CertificateRepository : ICertificateRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IHelpherMethods _helpherMethods;
        private readonly IimageRepository _iimageRepository;

        public CertificateRepository(IimageRepository iimageRepository,IHelpherMethods helpherMethods,ApplicationDbContext applicationDbContext, IUnitOfWork unitOfWork, IMapper mapper, IMemoryCacheRepository memoryCacheRepository)
        {
            _iimageRepository = iimageRepository;
            _context = applicationDbContext;
            _helpherMethods = helpherMethods;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCacheRepository = memoryCacheRepository;

        }
        public async Task<Result<CertificateGetDTOs>> DeleteCertificate(string certificateId, CancellationToken cancellationToken)
        {
            try
            {
                await _memoryCacheRepository.RemoveAsync(CacheKeys.Certificate);
                var certificate = await _unitOfWork.Repository<Certificate>().GetByIdAsync(certificateId);
                if(certificate is null)
                {
                    return Result<CertificateGetDTOs>.Failure("NotFound", "Certificate cannot be Found");

                }

                _unitOfWork.Repository<Certificate>().Delete(certificate);
                await _unitOfWork.SaveChangesAsync();
                return Result<CertificateGetDTOs>.Success(_mapper.Map<CertificateGetDTOs>(certificate));

            }
            catch(Exception ex)
            {
                throw new Exception("An error occured while Deleting");
            }
        }

        public async Task<Result<PagedResult<CertificateGetDTOs>>> GetAll(int pageIndex, int pageSize, CancellationToken cancellationToken)
         {
            try
            {
                var cacheKeys = CacheKeys.Certificate;
                var cacheData = await _memoryCacheRepository.GetCacheKey<PagedResult<CertificateGetDTOs>>(cacheKeys);
                if (cacheData is not null)
                {
                    return Result<PagedResult<CertificateGetDTOs>>.Success(cacheData);
                }

                //var imgs = await _context.certificates.Include(x => x.CertificateImages).ToListAsync();
                var certificateData = await _unitOfWork.Repository<Certificate>().GetAllAsyncWithPagination();
                var certificatePagedResult = await certificateData
                                        .Include(x => x.CertificateImages)
                                        .Select(x => new CertificateGetDTOs(
                                            x.Id,
                                            x.Grade,
                                            x.Type,
                                            x.Board,
                                            x.CertificateImages.Select(img => img.CertificateImgURL).ToList()
                                        ))
                                        .ToPagedResultAsync(pageIndex, pageSize);


                if (certificatePagedResult.Data.Items is null && certificateData.Any())
                {
                    return Result<PagedResult<CertificateGetDTOs>>.Failure("NotFound", "Certificate Data are not Found");

                }

                var certificateDataDTOs = _mapper.Map<PagedResult<CertificateGetDTOs>>(certificatePagedResult.Data);


                await _memoryCacheRepository.SetAsync(cacheKeys, certificateData, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);

                return Result<PagedResult<CertificateGetDTOs>>.Success(certificateDataDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Fetching all the data");
            }
        }




        public async Task<Result<CertificateGetDTOs>> GetById(string CertificateId, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = $"GetById{CertificateId}";
                var cacheData = await _memoryCacheRepository.GetCacheKey<CertificateGetDTOs>(cacheKey);
                if(cacheData is not null)
                {
                    return Result<CertificateGetDTOs>.Success(cacheData);
                }
                var certificateData = await _unitOfWork.Repository<Certificate>().GetByIdAsync(CertificateId);
                if(certificateData is null)
                {
                    return Result<CertificateGetDTOs>.Failure("NotFound", "Certificate Data are not Found");
                }
                await _memoryCacheRepository.SetAsync(cacheKey, certificateData, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {

                }, cancellationToken);
                return Result<CertificateGetDTOs>.Success(_mapper.Map<CertificateGetDTOs>(certificateData));

            }
            catch(Exception ex)
            {
                throw new Exception("An error occured while getting Certificate by Id");
            }
        }

        public async Task<Result<CertificateGetDTOs>> SaveCertificate(CertificateCreateDTOs certificateCreateDTOs, List<IFormFile> certificateFiles)
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
                        var certificateData = new Certificate(
                            newId,
                            certificateCreateDTOs.Grade,
                            certificateCreateDTOs.Type,
                            certificateCreateDTOs.Board
                        );

                        if (certificateData == null)
                        {
                            return Result<CertificateGetDTOs>.Failure("NotFound", "Error occurred while mapping");
                        }

        
                        certificateData.CreatedAt = DateTime.Now;
                        await _unitOfWork.Repository<Certificate>().AddAsync(certificateData);
                        await _unitOfWork.SaveChangesAsync();


                        var tasks = certificateFiles.Select(async item =>
                        {
                            string imageURL = await _iimageRepository.AddSingle(item);
                            return new CertificateImages(
                                Guid.NewGuid().ToString(), // Ensure each Id is unique
                                imageURL,
                                certificateData.Id
                            );
                        }).ToList();

                        // Await the completion of all tasks
                        var imagesDTOs = (await Task.WhenAll(tasks)).ToList();
                        await _unitOfWork.Repository<CertificateImages>().AddRange(imagesDTOs);
                        await _unitOfWork.SaveChangesAsync();
                      
                        // Map the certificate data to the result DTO and include image URLs
                        var resultDTO = new CertificateGetDTOs(
                            certificateData.Id,
                            certificateData.Grade,
                            certificateData.Type,
                            certificateData.Board,
                            imagesDTOs.Select(image => image.CertificateImgURL).ToList()
                        );

                        scope.Complete();

                        return Result<CertificateGetDTOs>.Success(_mapper.Map<CertificateGetDTOs>(resultDTO));


                    }
                    catch (Exception ex)
                    {
                        scope.Dispose();
                        throw new Exception("An error occured while Saving Data",ex);
                    }
                }

            }
            catch(Exception ex)
            {
                throw new Exception("An error occured while saving error", ex);
            }
        }

        public async Task<Result<CertificateGetDTOs>> UpdateCertificate(string CertificateId, CertificateUpdateDTOs certificateUpdateDTOs, List<IFormFile> multipleFiles)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                   
                    await _memoryCacheRepository.RemoveAsync(CacheKeys.Certificate);
                    if (string.IsNullOrEmpty(CertificateId))
                    {
                        return Result<CertificateGetDTOs>.Failure("Please provide a valid CertificateId");
                    }
                    var certificateDataToBeUpdated = await _unitOfWork.Repository<Certificate>().GetByIdAsync(CertificateId);
                    if (certificateDataToBeUpdated is null)
                    {
                        return Result<CertificateGetDTOs>.Failure("NotFound", "CitizenshipData are not Found");
                    }


                    List<string> certificateImages = _context.CertificateImages.Where(p => p.CertificateId == certificateDataToBeUpdated.Id).AsQueryable().AsNoTracking().Select(x => x.CertificateImgURL).ToList();

                    List<string> updatedImageUrl = await _iimageRepository.UpdateMultiple(multipleFiles, certificateImages);

                    List<CertificateImages> certificateImages1 = await _context.CertificateImages.Where(x => x.CertificateId == certificateDataToBeUpdated.Id).ToListAsync();

                    _unitOfWork.Repository<CertificateImages>().DeleteRange(certificateImages1);

                    List<CertificateImages> updateCertificateImg = new List<CertificateImages>();

                    var tasks = multipleFiles.Select(async item =>
                    {
                        string imageURL = await _iimageRepository.AddSingle(item);

                        return new CertificateImages(Guid.NewGuid().ToString(), imageURL, certificateDataToBeUpdated.Id);
                    }).ToList();

                    // Await the completion of all tasks

                    var results = await Task.WhenAll(tasks);
                    updateCertificateImg.AddRange(results);
                    certificateDataToBeUpdated.CertificateImages = updateCertificateImg;

                    //Bulk Update
                    _mapper.Map(certificateUpdateDTOs, certificateDataToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();

                
                    var resultDTO = new CertificateGetDTOs(
                      certificateDataToBeUpdated.Id,
                      certificateDataToBeUpdated.Grade,
                      certificateDataToBeUpdated.Type,
                      certificateDataToBeUpdated.Board,
                      updateCertificateImg.Select(x=>x.CertificateImgURL).ToList()
                  );


                    scope.Complete();

                    return Result<CertificateGetDTOs>.Success(_mapper.Map<CertificateGetDTOs>(resultDTO));

                    
                }

            }catch(Exception ex)
            {
                throw new Exception("An error occured while updating Certificate", ex);
            }
        }
    }
}
