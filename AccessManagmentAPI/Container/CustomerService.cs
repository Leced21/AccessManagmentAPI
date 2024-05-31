using AccessManagmentAPI.Context;
using AccessManagmentAPI.Helper;
using AccessManagmentAPI.Models;
using AccessManagmentAPI.Repos.Models;
using AccessManagmentAPI.Service;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AccessManagmentAPI.Container
{
    public class CustomerService:ICustomerService
    {
        private readonly ContetxtDb _contextDb;
        private readonly IMapper mapper;
        private readonly ILogger<CustomerService> logger;
        public CustomerService(ContetxtDb contextDb, IMapper mapper, ILogger<CustomerService> logger)
        {
            this._contextDb = contextDb;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<APIResponse> Create(Customermodal data)
        {
            APIResponse response = new APIResponse();
            try
            {
                this.logger.LogInformation("Create Begins");
                TblCustomer _customer = this.mapper.Map<Customermodal, TblCustomer>(data);
                await this._contextDb.TblCustomers.AddAsync(_customer);
                await this._contextDb.SaveChangesAsync();
                response.ResponseCode = 201;
                response.Result = "pass";
            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.Message = ex.Message;
                this.logger.LogError(ex.Message, ex);
            }
            return response;
        }

        public async Task<List<Customermodal>> Getall()
        {
            List<Customermodal> _response = new List<Customermodal>();
            var _data = await this._contextDb.TblCustomers.ToListAsync();
            if (_data != null)
            {
                _response = this.mapper.Map<List<TblCustomer>, List<Customermodal>>(_data);
            }
            return _response;
        }

        public async Task<Customermodal> Getbycode(string code)
        {
            Customermodal _response = new Customermodal();
            var _data = await this._contextDb.TblCustomers.FindAsync(code);
            if (_data != null)
            {
                _response = this.mapper.Map<TblCustomer, Customermodal>(_data);
            }
            return _response;
        }

        public async Task<APIResponse> Remove(string code)
        {
            APIResponse response = new APIResponse();
            try
            {
                var _customer = await this._contextDb.TblCustomers.FindAsync(code);
                if (_customer != null)
                {
                    this._contextDb.TblCustomers.Remove(_customer);
                    await this._contextDb.SaveChangesAsync();
                    response.ResponseCode = 200;
                    response.Result = "pass";
                }
                else
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not found";
                }

            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> Update(Customermodal data, string code)
        {
            APIResponse response = new APIResponse();
            try
            {
                var _customer = await this._contextDb.TblCustomers.FindAsync(code);
                if (_customer != null)
                {
                    _customer.Name = data.Name;
                    _customer.Email = data.Email;
                    _customer.Phone = data.Phone;
                    _customer.IsActive = data.IsActive;
                    _customer.Creditlimit = data.Creditlimit;
                    await this._contextDb.SaveChangesAsync();
                    response.ResponseCode = 200;
                    response.Result = "pass";
                }
                else
                {
                    response.ResponseCode = 404;
                    response.Message = "Data not found";
                }

            }
            catch (Exception ex)
            {
                response.ResponseCode = 400;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}

