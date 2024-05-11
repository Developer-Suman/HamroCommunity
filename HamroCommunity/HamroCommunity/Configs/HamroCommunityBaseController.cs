using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTOs.Authentication;
using Project.DLL.Models;
using System.Security.Claims;

namespace HamroCommunity.Configs
{
    [Route("api/[controller]")]
    [ApiController]
    public class HamroCommunityBaseController : ControllerBase
    {
        private UserDTOs? _currentUser;
        private string _userRole;
        private readonly UserManager<ApplicationUsers> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public HamroCommunityBaseController(IMapper mapper, UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            
        }

        protected async Task<UserDTOs> GetCurrentUser()
        {
            if(_currentUser is null)
            {
                var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(nameIdentifier))
                {
                    throw new InvalidDataException("Current user not Found");
                }
                var dbUser = _userManager.Users.FirstOrDefault(x=>x.Id == nameIdentifier);
                _currentUser = _mapper.Map<UserDTOs>(dbUser);
            }         
            return _currentUser;
        }

        protected string GetCurrentUserRoles()
        {
            if(_userRole is null)
            {
                var rolesIdentifier = User.FindFirst(ClaimTypes.Role)?.Value;
                if(string.IsNullOrEmpty(rolesIdentifier))
                {
                    throw new InvalidDataException("CurrentUser roles Not Found");
                }

                var UserRoles = _roleManager.Roles.FirstOrDefault(x=>x.Name == rolesIdentifier);
                _userRole = UserRoles.ToString();
            }
            return _userRole;
        }

        protected IActionResult HandleFailureResult(IEnumerable<string> errors)
        {
            // Check the error messages and return appropriate status code

            if (errors.Any(errors => errors.Contains("Unauthorized")))
            {
                return Unauthorized(errors);
            }
            else if (errors.Any(errors => errors.Contains("NotFound")))
            {
                return NotFound(errors);
            }
            else if (errors.Any(errors =>errors.Contains("InsufficientFunds")))
            {
                return StatusCode(402, errors);
            }
            else if (errors.Any(errors =>errors.Contains("ForbiddenAccess")))
            {
                return Forbid(string.Join(", ", errors));
            }
            else if(errors.Any(errors => errors.Contains("Conflict")))
            {
                return Conflict(errors);
            }
            else
            {
                return BadRequest(errors);
            }
        }
    }
}
