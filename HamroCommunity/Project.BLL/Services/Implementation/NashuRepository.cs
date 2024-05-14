using AutoMapper;
using Project.BLL.DTOs.Nashu;
using Project.BLL.Repository.Interface;
using Project.BLL.Services.Interface;
using Project.DLL.Abstraction;
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

        public NashuRepository(IMapper mapper, IMemoryCacheRepository memoryCacheRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _cacheRepository = memoryCacheRepository;
            _unitOfWork = unitOfWork;
            
        }
        public Task<Result<NashuGetDTOs>> DeleteNashuData(string NashuId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<NashuGetDTOs>>> GetAllNashuData(int page, int pageSize, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<NashuGetDTOs>> GetNashuDataById(string NashuId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<NashuGetDTOs>> SaveNashuData(NashuCreateDTOs nashuCreateDTOs)
        {
            throw new NotImplementedException();
        }

        public Task<Result<NashuGetDTOs>> UpdateNashuData(NashuUpdateDTOs nashuUpdateDTOs)
        {
            throw new NotImplementedException();
        }
    }
}
