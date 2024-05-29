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
        public async Task<IActionResult> confirmRegistration(int userid, string username, string otptext)
        {
            var data = await this.userService.ConfirmRegister(userid, username, otptext);
            return Ok(data);
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> resetpassword( string username, string oldpassword, string newpassword)
        {
            var data = await this.userService.ResetPassword( username, oldpassword, newpassword);
            return Ok(data);
        }

        [HttpPost("forgetpassword")]
        public async Task<IActionResult> forgetpassword(string username)
        {
            var data = await this.userService.ForgetPassword(username );
            return Ok(data);
        }
        [HttpPost("updatepassword")]
        public async Task<IActionResult> updatepassword(string username, string password, string otptext)
        {
            var data = await this.userService.UpdatePassword(username,password, otptext );
            return Ok(data);
        }



        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Userregister>>> Getalluser()
        //{
        //    if (_contextdb.Userregisters == null)
        //    {
        //        return NotFound();
        //    }
        //    return await _contextdb.Userregisters.ToListAsync();
        //}

        //[HttpGet("{id:int}")]
        //public async Task<IActionResult> GetById( int id)
        //{

        //    var user = await _contextdb.Userregisters.FindAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(user);
    }

        //[HttpPost("login")]
        //// [Authorize]
        //public async Task<IActionResult> Login(LoginDto loginDto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);
        //    var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());

        //    if (user == null) return Unauthorized("Invalid username !");
        //    var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        //    if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");
        //    return Ok(
        //        new NewUserDto
        //        {
        //            UserName = user.UserName,
        //            Email = user.Email,
        //            token = _tokenService.CreateToken(user)
        //        }
        //    );
        //}

        //[HttpPost("userregisteration")]
        //public async Task<ActionResult<Userregister>> userregistration(Userregister user)
        //{
        //    if (_contextdb.Userregisters == null)
        //    {
        //        return Problem("Entity set 'ContetxtDb.Userregisters'  is null.");
        //    }
        //    await _contextdb.Userregisters.AddAsync(user);
        //    await _contextdb.SaveChangesAsync();

        //    return CreatedAtAction("Getalluser", new { id = user.Id }, user);
        //}
    
}
