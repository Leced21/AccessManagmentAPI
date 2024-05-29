using AccessManagmentAPI.Context;
using AccessManagmentAPI.Helper;
using AccessManagmentAPI.Models;
using AccessManagmentAPI.Repos.Models;
using AccessManagmentAPI.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AccessManagmentAPI.Container
{
    public class UserService : IUserService
    {
        private readonly ContetxtDb _contetxtDb;
        public UserService(ContetxtDb contetxtdb)
        {
            _contetxtDb = contetxtdb;
        }

        public async Task<APIResponse> ConfirmRegister(int userid, string username, string otptext)
        {
            APIResponse response = new APIResponse();
            bool otpresponse=await ValidateOTP(username, otptext);
            if (!otpresponse) 
            {
                response.Result = "fail";
                response.Message = "Invalid OTP or Expired";
            }
            else
            {
                var _tempdata=await this._contetxtDb.TblTempusers.FirstOrDefaultAsync(item=>item.Id==userid);
                var _user = new TblUser()
                {
                    Username = username,
                    Name = _tempdata.Name,
                    Password = _tempdata.Password,
                    Email = _tempdata.Email,
                    Phone = _tempdata.Phone,
                    Failattempt = 0,
                    Isactive = true,
                    Islocked = false,
                    Role = "user"
                };
                await this._contetxtDb.TblUsers.AddAsync(_user);
                await this._contetxtDb.SaveChangesAsync();
                await UpdatePWDManager(username, _tempdata.Password);
                response.Result = "pass";
                response.Message = "Registered successfully";
            }

            return response;
        }

        public async Task<APIResponse> UserRegisteration(UserRegister userRegister)
        {

            APIResponse response = new APIResponse();
            int userid = 0;
            bool isvalid = true;

            try
            {
                //duplicate user
                var _user=await this._contetxtDb.TblUsers.Where(item=>item.Username==userRegister.UserName).ToListAsync();
                if (_user.Count > 0) 
                { 
                    isvalid = false;
                    response.Result = "fail";
                    response.Message = "Duplicate username";
                }

                // duplicate Email
                var _usermail = await this._contetxtDb.TblUsers.Where(item => item.Username == userRegister.Email).ToListAsync();
                if (_usermail.Count > 0)
                {
                    isvalid = false;
                    response.Result = "fail";
                    response.Message = "Duplicate email";
                }

                if (userRegister != null && isvalid)
                {
                    var _tempuser = new TblTempuser()
                    {
                        Code = userRegister.UserName,
                        Name = userRegister.Name,
                        Email = userRegister.Email,
                        Password = userRegister.Password,
                        Phone = userRegister.Phone,
                    };
                    await this._contetxtDb.TblTempusers.AddAsync(_tempuser);
                    await this._contetxtDb.SaveChangesAsync();
                    userid=_tempuser.Id;
                    string OTPText = Generaterandomnulber();
                    await UpdateOtp(userRegister.UserName, OTPText, "register");
                    await SendOtpMail(userRegister.Email, OTPText, userRegister.Name);
                    response.Result = "pass";
                    response.Message=userid.ToString();
                }
            }
            catch (Exception ex)
            {
                response.Result = "fail";
            }
            return response;


        }

        private async Task UpdateOtp(string username,string otptext, string otptype)
        {
            var _otp = new TblOtpManager()
            {
                Username = username,
                Otptext = otptext,
                Expiration = DateTime.Now.AddMinutes(30),
                Createddate = DateTime.Now,
                Otptype = otptype
            };
            await this._contetxtDb.TblOtpManagers.AddAsync(_otp);
            await this._contetxtDb.SaveChangesAsync();
        }

        private async Task<bool> ValidateOTP(string username, string OTPText)
        {
            bool response = false;
            var _data = await this._contetxtDb.TblOtpManagers.FirstOrDefaultAsync(item => item.Username == username && item.Otptext == OTPText && item.Expiration > DateTime.Now);
            if (_data != null)
            {
                response = true;
            }
            return response;
        }

        private async Task UpdatePWDManager(string username, string password)
        {
            var _otp = new TblPwdManger()
            {
                Username = username,
                Password = password,
                ModifyDate = DateTime.Now,
            };
            await this._contetxtDb.TblPwdMangers.AddAsync(_otp);
            await this._contetxtDb.SaveChangesAsync();
        }

        private string Generaterandomnulber()
        {
            Random random= new Random();    
            string randomno=random.Next(0, 1000000).ToString("D6");
            return randomno;
        }

        private async Task SendOtpMail(string usermail, string OtpText, string Name)
        {

        }

        public async Task<APIResponse> ResetPassword(string username, string oldpassword, string newpassword)
        {
            APIResponse response = new APIResponse();
            var _user = await this._contetxtDb.TblUsers.FirstOrDefaultAsync(item => item.Username == username && item.Password == oldpassword && item.Isactive == true);
            if (_user != null) 
            {
                var _pwdhistory = await Validatepwdhistory(username, newpassword);
                if (_pwdhistory)
                {
                    response.Result = "Fail";
                    response.Message = "Don't use the same password that used in last 3 transaction";
                }
                else
                {
                    _user.Password = newpassword;
                    await this._contetxtDb.SaveChangesAsync();
                    await UpdatePWDManager(username, newpassword);
                    response.Result = "pass";
                    response.Message = "Password changed.";
                }
            }
            else
            {
                response.Result = "Fail";
                response.Message = "Failed to validate old password.";
            }
            return response;
        }

        private async Task <bool>Validatepwdhistory(string Username, string password)
        {
            bool response = false;
            var _pwd=await this._contetxtDb.TblPwdMangers.Where(item=>item.Username==Username).OrderByDescending(p=>p.ModifyDate).Take(3).ToListAsync();
            if (_pwd.Count>0)
            {
                var validate = _pwd.Where(o => o.Password == password);
                if (validate.Any())
                {
                    response = true;
                }
            }
            return response;
        }
    }
}
