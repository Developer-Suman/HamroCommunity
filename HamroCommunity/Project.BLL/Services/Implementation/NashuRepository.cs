﻿using AutoMapper;
using Project.BLL.DTOs.Nashu;
using Project.BLL.Repository.Interface;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
using Project.DLL.DbContext;
using Project.DLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Implementation
{
    public class NashuRepository : INashuRepository
    {

        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _cacheRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;

        public NashuRepository(ApplicationDbContext context,IMapper mapper, IMemoryCacheRepository memoryCacheRepository, IUnitOfWork unitOfWork)
        {
            _context = context;
            _mapper = mapper;
            _cacheRepository = memoryCacheRepository;
            _unitOfWork = unitOfWork;
            
        }
        public async Task<Result<NashuGetDTOs>> DeleteNashuData(string NashuId)
        {
            try
            {
                var nashuData = await _unitOfWork.Repository<Nashu>().GetByIdAsync(NashuId);
                if(nashuData is null)
                {
                    return Result<NashuGetDTOs>.Failure("NotFound", "Nashu Data Not found to be deleted");
                }
                _unitOfWork.Repository<Nashu>().Delete(nashuData);
                await _unitOfWork.SaveChangesAsync();
                return Result<NashuGetDTOs>.Success(_mapper.Map<NashuGetDTOs>(nashuData));

            }catch(Exception ex)
            {
                throw new Exception("An error occured while deleting");
            }
        }

        public Task<Result<List<NashuGetDTOs>>> GetAllNashuData(int page, int pageSize, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<NashuGetDTOs>> GetNashuDataById(string NashuId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<NashuGetDTOs>> SaveNashuData(NashuCreateDTOs nashuCreateDTOs)
        {
            try
            {
                var nashuData = _mapper.Map<Nashu>(nashuCreateDTOs);
                if(nashuData is null)
                {
                    return Result<NashuGetDTOs>.Failure("Error occured while mapping Entity");
                }
                await _unitOfWork.Repository<Nashu>().AddAsync(nashuData);
                await _unitOfWork.SaveChangesAsync();

                return Result<NashuGetDTOs>.Success(_mapper.Map<NashuGetDTOs>(nashuData));

            }catch(Exception ex)
            {
                throw new Exception("An exception occured while Adding Nashu Data");
            }
        }

        public Task<Result<NashuGetDTOs>> UpdateNashuData(NashuUpdateDTOs nashuUpdateDTOs)
        {
            throw new NotImplementedException();
        }
    }
}
