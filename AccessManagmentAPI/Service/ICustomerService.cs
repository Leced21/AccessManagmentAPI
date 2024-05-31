using AccessManagmentAPI.Helper;
using AccessManagmentAPI.Models;

namespace AccessManagmentAPI.Service
{
    public interface ICustomerService
    {
        Task<List<Customermodal>> Getall();
        Task<Customermodal> Getbycode(string code);
        Task<APIResponse> Remove(string code);
        Task<APIResponse> Create(Customermodal data);
        Task<APIResponse> Update (Customermodal data, string code);
    }
}
