using AccessManagmentAPI.Context;
using AccessManagmentAPI.Helper;
using AccessManagmentAPI.Models;
using AccessManagmentAPI.Repos.Models;
using AccessManagmentAPI.Service;
using Microsoft.AspNetCore.Identity;

namespace AccessManagmentAPI.Container
{
    public class UserService : IUserService
    {
        private readonly ContetxtDb _contetxtDb;
        public UserService(ContetxtDb contetxtdb)
        {
            _contetxtDb = contetxtdb;
        }

        public Task<APIResponse> ConfirmRegister(int userid, string username, string otptext)
        {
            throw new NotImplementedException();
        }

        public async Task<APIResponse> UserRegisteration(UserRegister userRegister)
        {

            APIResponse response = new APIResponse();
            int userid = 0;
            try
            {
                if (userRegister != null)
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

        private string Generaterandomnulber()
        {
            Random random= new Random();    
            string randomno=random.Next(0, 1000000).ToString("D6");
            return randomno;
        }

        private async Task SendOtpMail(string usermail, string OtpText, string Name)
        {

        }
    }
}
