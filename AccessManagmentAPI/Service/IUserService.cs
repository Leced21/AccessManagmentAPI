using AccessManagmentAPI.Helper;
using AccessManagmentAPI.Models;

namespace AccessManagmentAPI.Service
{
    public interface IUserService
    {
        Task<APIResponse> UserRegisteration(UserRegister userRegister);
        Task<APIResponse> ConfirmRegister(int userid, string username, string otptext);
        Task<APIResponse> ResetPassword(string username, string oldpassword, string newpassword);
        Task<APIResponse> FPassword(string username, string oldpassword, string newpassword);
    }
}
