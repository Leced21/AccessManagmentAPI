using AccessManagmentAPI.Context;
using AccessManagmentAPI.Repos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AccessManagmentAPI.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class AssociateController : ControllerBase
        {
            private readonly ContetxtDb _contetxtDb;
            public AssociateController(ContetxtDb contetxtDb)
            {
                this._contetxtDb = contetxtDb;
            }

            [HttpGet("getall")]
            public async Task<ActionResult> Getall()
            {
                string sqlquery = "exec sp_getcustomer";
                var data = await this._contetxtDb.TblCustomers.FromSqlRaw(sqlquery).ToListAsync();
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);

            }

            [HttpGet("Getallcustom")]
            public async Task<ActionResult> Getallcustom()
            {
                string sqlquery = "exec sp_getcustomer_custom";
                var data = await this._contetxtDb.customerdetail.FromSqlRaw(sqlquery).ToListAsync();
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);

            }

            [HttpGet("Getallcustomq")]
            public async Task<ActionResult> Getallcustomq()
            {
                string sqlquery = "Select *,'Active' as Statusname from tbl_customer";
                var data = await this._contetxtDb.customerdetail.FromSqlRaw(sqlquery).ToListAsync();
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);

            }

            [HttpGet("getbycode")]
            public async Task<ActionResult> getbycode(string code)
            {
                string sqlquery = "Select *,'Active' as Statusname from tbl_customer where code=@code";
                SqlParameter parameter = new SqlParameter("@code", code);
                var data = await this._contetxtDb.customerdetail.FromSqlRaw(sqlquery, parameter).FirstOrDefaultAsync();
                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);

            }

            [HttpPost("create")]
            public async Task<ActionResult> create(TblCustomer tblCustomer)
            {
                string sqlquery = "Insert Into tbl_customer values(@code,@name,@email,@phone,@creditlimit,@active,@taxcode)";
                SqlParameter[] parameter =
                {
                new SqlParameter("@code",tblCustomer.Code),
                new SqlParameter("@name",tblCustomer.Name),
                new SqlParameter("@email",tblCustomer.Email),
                new SqlParameter("@phone",tblCustomer.Phone),
                new SqlParameter("@creditlimit",tblCustomer.Creditlimit),
                new SqlParameter("@active",tblCustomer.IsActive),
                new SqlParameter("@taxcode",tblCustomer.Taxcode)
            };
                var data = await this._contetxtDb.Database.ExecuteSqlRawAsync(sqlquery, parameter);
                return Ok(data);

            }

            [HttpPut("Update")]
            public async Task<ActionResult> Update(string code, TblCustomer tblCustomer)
            {
                string sqlquery = "exec sp_createcustomer @code,@name,@email,@phone,@creditlimit,@active,@taxcode,@type";
                SqlParameter[] parameter =
                {
                new SqlParameter("@code",code),
                new SqlParameter("@name",tblCustomer.Name),
                new SqlParameter("@email",tblCustomer.Email),
                new SqlParameter("@phone",tblCustomer.Phone),
                new SqlParameter("@creditlimit",tblCustomer.Creditlimit),
                new SqlParameter("@active",tblCustomer.IsActive),
                new SqlParameter("@taxcode",tblCustomer.Taxcode),
                new SqlParameter("@type","update")
            };
                var data = await this._contetxtDb.Database.ExecuteSqlRawAsync(sqlquery, parameter);
                return Ok(data);

            }

            [HttpDelete("delete")]
            public async Task<ActionResult> delete(string code)
            {
                string sqlquery = "exec sp_deletecustomer @code";
                SqlParameter[] parameter =
                {
                new SqlParameter("@code",code)
            };
                var data = await this._contetxtDb.Database.ExecuteSqlRawAsync(sqlquery, parameter);
                return Ok(data);

            }


        }
}

