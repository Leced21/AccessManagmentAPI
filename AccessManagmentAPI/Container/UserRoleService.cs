using AccessManagmentAPI.Context;
using AccessManagmentAPI.Helper;
using AccessManagmentAPI.Models;
using AccessManagmentAPI.Repos.Models;
using AccessManagmentAPI.Service;
using Microsoft.EntityFrameworkCore;

namespace AccessManagmentAPI.Container
{
    public class UserRoleService:IUserRoleServices
    {
        private readonly ContetxtDb _contetxtDb;
        public UserRoleService(ContetxtDb contetxtDb)
        {
            _contetxtDb = contetxtDb;
        }
        public async Task<APIResponse> AssignRolePermission(List<TblRolepermission> _data)
        {
            APIResponse response = new APIResponse();
            int processcount = 0;
            try
            {
                using (var dbtransaction = await this._contetxtDb.Database.BeginTransactionAsync()) 
                {
                    if (_data.Count > 0)
                    {
                        _data.ForEach(item =>
                        {
                            var userdata = this._contetxtDb.TblRolepermissions.FirstOrDefault(item => item.Userrole == item.Userrole && item.Menucode == item.Menucode);
                            if (userdata!=null)
                            {
                                userdata.Haveview=item.Haveview;
                                userdata.Haveadd=item.Haveadd;
                                userdata.Havedelete=item.Havedelete;
                                userdata.Haveedit=item.Haveedit;
                                processcount++;
                            }
                            else
                            {
                                this._contetxtDb.TblRolepermissions.Add(item);
                                processcount++;
                            }
                        });
                        if (_data.Count == processcount)
                        {
                            await this._contetxtDb.SaveChangesAsync();
                            await dbtransaction.CommitAsync();
                            response.Result = "Pass";
                            response.Message = "Saved successfully";
                        }
                        else
                        {
                            await dbtransaction.RollbackAsync();
                        }
                    }
                    else
                    {
                        response.Result = "Fail";
                        response.Message = "please proceed with mminimum 1 record";
                    }
                }
               
            }
            catch (Exception ex)
            {

            }
           
            return response;
        }

        public async Task<List<Appmenu>> GetAllMenusbyrole(string userrole)
        {
            List<Appmenu> appmenus = new List<Appmenu>();
            var accessdata = (from menu in this._contetxtDb.TblRolepermissions.Where(o => o.Userrole == userrole && o.Haveview) 
                              join m in this._contetxtDb.TblMenus on menu.Menucode equals m.Code into _jointable
                              from p in _jointable.DefaultIfEmpty()
                              select new {code=menu.Menucode, name=p.Name}).ToList();
            if (accessdata.Any())
            {
                accessdata.ForEach(item =>
                {
                    appmenus.Add(new Appmenu()
                    {
                        code = item.code,
                        Name = item.name,
                    });
                });
            }
            return appmenus;
        }


        public async Task<List<TblMenu>> GetAllMenus()
        {
            return await this._contetxtDb.TblMenus.ToListAsync();
        }

        public async Task<List<TblRole>> GetAllRoles()
        {
           return await this._contetxtDb.TblRoles.ToListAsync();
        }

        public async Task<Menupermission>GetMenupermissionbyrole(string userrole, string menucode)
        {
            Menupermission menupermission = new Menupermission();
            var _data = await this._contetxtDb.TblRolepermissions.FirstOrDefaultAsync(o => o.Userrole == userrole && o.Haveview && o.Menucode == menucode);
            if (_data != null)
            {
                menupermission.code = _data.Menucode;
                menupermission.Haveview = _data.Haveview;
                menupermission.Haveadd = _data.Haveadd;
                menupermission.Haveedit = _data.Haveedit;
                menupermission.Havedelete = _data.Havedelete;
            }
            return menupermission;
        }
    }
}
