using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.BLL.DTOs.Citizenship;
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
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Project.BLL.Services.Implementation
{
    public class CitizenshipRepository : ICitizenshipRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly IimageRepository _imageRepository;
        private readonly ApplicationDbContext _context;



        public CitizenshipRepository(ApplicationDbContext context,IUnitOfWork unitOfWork, IMapper mapper, IMemoryCacheRepository memoryCacheRepository, IimageRepository imageRepository)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCacheRepository = memoryCacheRepository;
            _imageRepository = imageRepository;

        }
        public async Task<Result<CitizenshipGetDTOs>> DeleteCitizenshipData(string CitizenshipId)
        {
            try
            {
                await _memoryCacheRepository.RemoveAsync(CacheKeys.Citizenship);
                var citizenshipData = await _unitOfWork.Repository<Citizenship>().GetByIdAsync(CitizenshipId);
                if (citizenshipData is null)
                {
                    return Result<CitizenshipGetDTOs>.Failure("NotFound", "Citizenship Data Not found to be deleted");
                }
                _unitOfWork.Repository<Citizenship>().Delete(citizenshipData);
                await _unitOfWork.SaveChangesAsync();
                return Result<CitizenshipGetDTOs>.Success(_mapper.Map<CitizenshipGetDTOs>(citizenshipData));

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while deleting");
            }
        }

        public async Task<Result<PagedResult<CitizenshipGetDTOs>>> GetAllCitizenshipData(int pageIndex, int pageSize, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKeys = CacheKeys.Citizenship;
                var cacheData = await _memoryCacheRepository.GetCacheKey<PagedResult<CitizenshipGetDTOs>>(cacheKeys);


                //DateTime date = DateTime.Now;
                //DateTime specificDate = new DateTime(1998, 5, 30);
                //var exectDate = _helpherMethods.CalculateAge(specificDate, date);

                if (cacheData is not null)
                {
                    return Result<PagedResult<CitizenshipGetDTOs>>.Success(cacheData);
                }

                //var imageInclude = await _context.citizenships.Include(x => x.CitizenshipImages).ToListAsync();
                var citizenshipData = await _unitOfWork.Repository<Citizenship>().GetAllAsyncWithPagination();
                var citizenshipPaginatedResult = await citizenshipData
                    .Include(x=>x.CitizenshipImages)
                    .Select(x=> new CitizenshipGetDTOs(
                        x.Id,
                        x.IssuedDate,
                        x.IssuedDistrict,
                        x.VdcOrMunicipality,
                        x.WardNumber,
                        x.DOB,
                        x.CitizenshipNumber,
                        x.CitizenshipImages.Select(img => img.ImageUrl).ToList()
                        )).ToPagedResultAsync(pageIndex, pageSize);



                if (citizenshipPaginatedResult.Data.Items is null && citizenshipData.Any())
                {
                    return Result<PagedResult<CitizenshipGetDTOs>>.Failure("NotFound", "Certificate Data are not Found");
                };

                var citizenshipDataDTOs = _mapper.Map<PagedResult<CitizenshipGetDTOs>>(citizenshipPaginatedResult.Data);
                await _memoryCacheRepository.SetAsync(cacheKeys, citizenshipDataDTOs, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);

                return Result<PagedResult<CitizenshipGetDTOs>>.Success(citizenshipDataDTOs);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while Fetching all Data");
            }
        }

        public async Task<Result<CitizenshipGetDTOs>> GetCitizenshipDataById(string CitizenshipId, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKeys = $"GetCitizenshipDataById{CitizenshipId}";
                var cacheData = await _memoryCacheRepository.GetCacheKey<CitizenshipGetDTOs>(cacheKeys);

                if (cacheData is not null)
                {
                    return Result<CitizenshipGetDTOs>.Success(cacheData);
                }
                if (CitizenshipId is null)
                {
                    return Result<CitizenshipGetDTOs>.Failure("Please provide CitizenshipId");

                }

                var citizenshipData = _context.Citizenships
                .AsNoTracking()
                .Include(c => c.CitizenshipImages) 
                .FirstOrDefault(c => c.Id == CitizenshipId);

                var resultDTOs = new CitizenshipGetDTOs(
                citizenshipData.Id,
                citizenshipData.IssuedDate,
                citizenshipData.IssuedDistrict,
                citizenshipData.VdcOrMunicipality,
                citizenshipData.WardNumber,
                citizenshipData.DOB,
                citizenshipData.CitizenshipNumber,
                citizenshipData?.CitizenshipImages.Select(ci => ci.ImageUrl).ToList()
                );




                await _memoryCacheRepository.SetAsync(cacheKeys, citizenshipData, new Microsoft.Extensions.Caching.Memory.MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30)
                }, cancellationToken);



                return Result<CitizenshipGetDTOs>.Success(_mapper.Map<CitizenshipGetDTOs>(resultDTOs));

            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while fetching nashu data");
            }

        }

        public async Task<Result<CitizenshipGetDTOs>> SaveCitizenshipData(CitizenshipCreateDTOs citizenshipCreateDTOs, List<IFormFile> certificateFiles)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _memoryCacheRepository.RemoveAsync(CacheKeys.Citizenship);
                    string newId = Guid.NewGuid().ToString();
                    var citizenshipData = new Citizenship(
                        newId,
                        citizenshipCreateDTOs.IssuedDate,
                        citizenshipCreateDTOs.IssuedDistrict,
                        citizenshipCreateDTOs.VDCOrMunicipality,
                        citizenshipCreateDTOs.WardNumber,
                        citizenshipCreateDTOs.DOB,
                        citizenshipCreateDTOs.CitizenshipNumber
                        );


                    if (citizenshipData is null)
                    {
                        return Result<CitizenshipGetDTOs>.Failure("NotFound", "Error occurred while mapping");
                    }

                  
                    //citizenshipData.DocumentsId = citizenshipCreateDTOs.DocumentsId;
                    await _unitOfWork.Repository<Citizenship>().AddAsync(citizenshipData);
                    await _unitOfWork.SaveChangesAsync();

                    var tasks = certificateFiles.Select(async item =>
                    {
                        string imageURL = await _imageRepository.AddSingle(item);
                        return new CitizenshipImages(
                            Guid.NewGuid().ToString(),
                            imageURL, DateTime.Now,
                            citizenshipData.Id);
                    }).ToList();

                    // Await the completion of all tasks
                    var imagesDTOs = (await Task.WhenAll(tasks)).ToList();
                    await _unitOfWork.Repository<CitizenshipImages>().AddRange(imagesDTOs);
                    await _unitOfWork.SaveChangesAsync();
                    var imageUrls = imagesDTOs.Select(image=> image.ImageUrl).ToList();

                    //Map the citizenship data to resultDTOs and include imageUrls
                    var resultDTOs = new CitizenshipGetDTOs(
                        citizenshipData.Id,
                        citizenshipData.IssuedDate,
                        citizenshipData.IssuedDistrict,
                        citizenshipData.VdcOrMunicipality,
                        citizenshipData.WardNumber,
                        citizenshipData.DOB,
                        citizenshipData.CitizenshipNumber,
                        imageUrls
                        );

                    scope.Complete();

                    return Result<CitizenshipGetDTOs>.Success(_mapper.Map<CitizenshipGetDTOs>(resultDTOs));

                }
                catch (Exception ex)
                {
                    scope.Dispose();

                    throw new Exception("An exception occured while Adding Nashu Data");
                }

            }
        }

        public async Task<Result<CitizenshipGetDTOs>> UpdateCitizenshipData(string CitizenshipId, CitizenshipUpdateDTOs citizenshipUpdateDTOs)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _memoryCacheRepository.RemoveAsync(CacheKeys.Citizenship);
                    if (string.IsNullOrEmpty(CitizenshipId))
                    {
                        return Result<CitizenshipGetDTOs>.Failure("Please provide a valid CitizenshipId");
                    }
                    var citizenDataToBeUpdated = await _unitOfWork.Repository<Citizenship>().GetByIdAsync(CitizenshipId);
                    if (citizenDataToBeUpdated is null)
                    {
                        return Result<CitizenshipGetDTOs>.Failure("NotFound", "CitizenshipData are not Found");
                    }


                    List<string> citizenshipImages = _context.CitizenshipImages.Where(p=>p.CitizenshipId == citizenDataToBeUpdated.Id).AsQueryable().AsNoTracking().Select(x=>x.ImageUrl).ToList();


                    List<CitizenshipImages> imagesDTOs = new List<CitizenshipImages>();
                    // Process each new image
                    //foreach (var item in citizenshipListImages)
                    //{
                    //    // Update single image
                    //    string imageURL = await _imageRepository.UpdateSingle(item.FormFile, citizenshipImages.SingleOrDefault());

                    //    // Create a new CitizenshipImages object
                    //    CitizenshipImages newImage = new CitizenshipImages(Guid.NewGuid().ToString(), imageURL, DateTime.Now, citizenshipUpdateDTOs.Id);

                    //    // Add to the list
                    //    imagesDTOs.Add(newImage);
                    //}


                    // Await the completion of all tasks
                    citizenDataToBeUpdated.CitizenshipImages = imagesDTOs;

                    //Bulk Update
                    _mapper.Map(citizenshipUpdateDTOs, citizenDataToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    return Result<CitizenshipGetDTOs>.Success(_mapper.Map<CitizenshipGetDTOs>(citizenshipUpdateDTOs));

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
