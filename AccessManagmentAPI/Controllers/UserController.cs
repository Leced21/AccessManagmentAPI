using AccessManagmentAPI.Context;
using AccessManagmentAPI.Helper;
using AccessManagmentAPI.Models;
using AccessManagmentAPI.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccessManagmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController:ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService service)
        {
            this.userService = service;
        }

        [HttpPost("userregisteration")]
        public async Task<IActionResult> UserRegistration(UserRegister userRegister)
        {
            var data =await this.userService.UserRegisteration(userRegister);
            return Ok(data);
        }

        [HttpPost("confirmregisteration")]
        public async Task<IActionResult> confirmRegistration(UserConfirm _data)
        {
            var data = await this.userService.ConfirmRegister(_data.userid, _data.username, _data.otptext);
            return Ok(data);
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> resetpassword( Resetpassword _data)
        {
            var data = await this.userService.ResetPassword( _data.username, _data.oldpassword, _data.newpassword);
            return Ok(data);
        }

        [HttpGet("forgetpassword")]
        public async Task<IActionResult> forgetpassword(string username)
        {
            var data = await this.userService.ForgetPassword(username );
            return Ok(data);
        }
        [HttpPost("updatepassword")]
        public async Task<IActionResult> updatepassword(Updatepassword _data)
        {
            var data = await this.userService.UpdatePassword(_data.username,_data.password, _data.otptext );
            return Ok(data);
        }

        [HttpPost("updatestatus")]
        public async Task<IActionResult> updatestatus(string username, bool status)
        {
            var data = await this.userService.UpdateStatus(username, status);
            return Ok(data);
        }

        [HttpPost("updaterole")]
        public async Task<IActionResult> updaterole(string username, string role)
        {
            var data = await this.userService.UpdateRole(username, role);
            return Ok(data);
        }
    }
}
