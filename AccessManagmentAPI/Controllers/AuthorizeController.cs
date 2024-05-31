using AccessManagmentAPI.Context;
using AccessManagmentAPI.Models;
using AccessManagmentAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AccessManagmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {  
            private readonly ContetxtDb _contextdb;
            private readonly JwtSettings _jwtSettings;
            private readonly IRefreshHandler _refresh;
            public AuthorizeController(ContetxtDb contextDb, IOptions<JwtSettings> options, IRefreshHandler refresh)
            {
                this._contextdb = contextDb;
                this._jwtSettings = options.Value;
                this._refresh = refresh;
            }
            [HttpPost("GenerateToken")]
            public async Task<IActionResult> GenerateToken([FromBody] UserCred userCred)
            {
                var user = await this._contextdb.TblUsers.FirstOrDefaultAsync(item => item.Username == userCred.username && item.Password == userCred.password && item.Isactive == true);
                if (user != null)
                {
                    //generate token
                    var tokenhandler = new JwtSecurityTokenHandler();
                    var tokenkey = Encoding.UTF8.GetBytes(this._jwtSettings.securitykey);
                    var tokendesc = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim(ClaimTypes.Name,user.Username),
                        new Claim(ClaimTypes.Role,user.Role)
                        }),
                        Expires = DateTime.UtcNow.AddSeconds(3000),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
                    };
                    var token = tokenhandler.CreateToken(tokendesc);
                    var finaltoken = tokenhandler.WriteToken(token);
                    return Ok(new TokenResponse() { Token = finaltoken, RefreshToken = await this._refresh.GenerateToken(userCred.username), UserRole = user.Role });

                }
                else
                {
                    return Unauthorized();
                }
            }

            [HttpPost("GenerateRefreshToken")]
            public async Task<IActionResult> GenerateToken([FromBody] TokenResponse token)
            {
                var _refreshtoken = await this._contextdb.TblRefreshtokens.FirstOrDefaultAsync(item => item.Refreshtoken == token.RefreshToken);
                if (_refreshtoken != null)
                {
                    //generate token
                    var tokenhandler = new JwtSecurityTokenHandler();
                    var tokenkey = Encoding.UTF8.GetBytes(this._jwtSettings.securitykey);
                    SecurityToken securityToken;
                    var principal = tokenhandler.ValidateToken(token.Token, new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(tokenkey),
                        ValidateIssuer = false,
                        ValidateAudience = false,

                    }, out securityToken);

                    var _token = securityToken as JwtSecurityToken;
                    if (_token != null && _token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
                    {
                        string username = principal.Identity?.Name;
                        var _existdata = await this._contextdb.TblRefreshtokens.FirstOrDefaultAsync(item => item.Userid == username
                        && item.Refreshtoken == token.RefreshToken);
                        if (_existdata != null)
                        {
                            var _newtoken = new JwtSecurityToken(
                                claims: principal.Claims.ToArray(),
                                expires: DateTime.Now.AddSeconds(30),
                                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._jwtSettings.securitykey)),
                                SecurityAlgorithms.HmacSha256)
                                );

                            var _finaltoken = tokenhandler.WriteToken(_newtoken);
                            return Ok(new TokenResponse() { Token = _finaltoken, RefreshToken = await this._refresh.GenerateToken(username), UserRole = token.UserRole });
                        }
                        else
                        {
                            return Unauthorized();
                        }
                    }
                    else
                    {
                        return Unauthorized();
                    }

                    //var tokendesc = new SecurityTokenDescriptor
                    //{
                    //    Subject = new ClaimsIdentity(new Claim[]
                    //    {
                    //        new Claim(ClaimTypes.Name,user.Code),
                    //        new Claim(ClaimTypes.Role,user.Role)
                    //    }),
                    //    Expires = DateTime.UtcNow.AddSeconds(30),
                    //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
                    //};
                    //var token = tokenhandler.CreateToken(tokendesc);
                    //var finaltoken = tokenhandler.WriteToken(token);
                    //return Ok(new TokenResponse() { Token = finaltoken, RefreshToken = await this.refresh.GenerateToken(userCred.username) });

                }
                else
                {
                    return Unauthorized();
                }

            }
        }
    
}
