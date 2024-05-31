using AccessManagmentAPI.Context;
using AccessManagmentAPI.Helper;
using AccessManagmentAPI.Models;
using AccessManagmentAPI.Repos.Models;

namespace AccessManagmentAPI.Service
{
    public interface IUserRoleServices
    {
        Task<APIResponse> AssignRolePermission (List<TblRolepermission> _data);
        Task<List<TblRole>> GetAllRoles();
        Task<List<TblMenu>> GetAllMenus();
        Task<List<Appmenu>> GetAllMenusbyrole(string userrole);
        Task<Menupermission> GetMenupermissionbyrole(string userrole,string menucode);
    }
}
