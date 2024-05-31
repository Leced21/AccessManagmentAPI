using AccessManagmentAPI.Models;
using AccessManagmentAPI.Repos.Models;
using AccessManagmentAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccessManagmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleServices _userRole;
        public UserRoleController(IUserRoleServices roleServices)
        {
            _userRole = roleServices;
        }

        [HttpPost("assignrolepermission")]
        public async Task<IActionResult> assignrolepermission(List<TblRolepermission> rolepermission)
        {
            var data =await this._userRole.AssignRolePermission(rolepermission);
            return Ok(data);
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var data = await this._userRole.GetAllRoles();
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpGet("GetAllMenus")]
        public async Task<IActionResult> GetAllMenus()
        {
            var data = await this._userRole.GetAllMenus();
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpGet("GetAllMenusbyrole")]
        public async Task<IActionResult> GetAllMenusbyrole(string userrole)
        {
            var data = await this._userRole.GetAllMenusbyrole(userrole);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpGet("GetMenupermissionbyrole")]
        public async Task<IActionResult> GetAllMGetMenupermissionbyroleenubyrole(string userrole, string menucode)
        {
            var data = await this._userRole.GetMenupermissionbyrole(userrole, menucode);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }
    }
}
