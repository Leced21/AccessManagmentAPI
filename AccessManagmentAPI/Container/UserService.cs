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

        }

        public Task<APIResponse> ConfirmRegister(int userid, string username, string otptext)
        {
            throw new NotImplementedException();
        }

        public async Task<APIResponse> UserRegisteration(UserRegister userRegister)
        {
            APIResponse response = new APIResponse();
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
                }
            }
            catch (Exception ex)
            {
                response = new APIResponse();
            }
            return response;


        }
    }
}
