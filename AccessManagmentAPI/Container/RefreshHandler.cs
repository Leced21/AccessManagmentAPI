using AccessManagmentAPI.Context;
using AccessManagmentAPI.Repos.Models;
using AccessManagmentAPI.Service;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AccessManagmentAPI.Container
{
    public class RefreshHandler:IRefreshHandler
    {
        private readonly ContetxtDb _contetxtDb;
        public RefreshHandler(ContetxtDb contextDb)
        {
            this._contetxtDb = contextDb;
        }
        public async Task<string> GenerateToken(string username)
        {
            var randomnumber = new byte[32];
            using (var randomnumbergenerator = RandomNumberGenerator.Create())
            {
                randomnumbergenerator.GetBytes(randomnumber);
                string refreshtoken = Convert.ToBase64String(randomnumber);
                var Existtoken = this._contetxtDb.TblRefreshtokens.FirstOrDefaultAsync(item => item.Userid == username).Result;
                if (Existtoken != null)
                {
                    Existtoken.Refreshtoken = refreshtoken;
                }
                else
                {
                    await this._contetxtDb.TblRefreshtokens.AddAsync(new TblRefreshtoken
                    {
                        Userid = username,
                        Tokenid = new Random().Next().ToString(),
                        Refreshtoken = refreshtoken
                    });
                }
                await this._contetxtDb.SaveChangesAsync();

                return refreshtoken;

            }
        }
    }
}
