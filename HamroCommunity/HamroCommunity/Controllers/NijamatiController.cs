using AutoMapper;
using HamroCommunity.Configs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTOs.Nashu;
using Project.BLL.Services.Interface;
using Project.DLL.Models;

namespace HamroCommunity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NijamatiController : HamroCommunityBaseController
    {
        private readonly INashuRepository _nashuRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;

        public NijamatiController(IMemoryCacheRepository memoryCacheRepository, IMapper mapper, INashuRepository nashuRepository, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager) : base(mapper, userManager, roleManager)
        {
            _memoryCacheRepository = memoryCacheRepository;
            _mapper = mapper;
            _nashuRepository = nashuRepository;

        }
        [HttpPost("SaveNashu")]
        public async Task<IActionResult> SaveNashu([FromForm] NashuCreateDTOs nashuCreateDTOs)
        {
            var saveNashuResult = await _nashuRepository.SaveNashuData(nashuCreateDTOs);

            #region Switch
            return saveNashuResult switch
            {
                { IsSuccess: true, Data: not null} => CreatedAtAction(nameof(SaveNashu), saveNashuResult.Data),
                { IsSuccess: false, Errors: not null} => HandleFailureResult(saveNashuResult.Errors),
                _ => BadRequest("Invalid Some Fields")
            };
            #endregion

        }
    }
}
