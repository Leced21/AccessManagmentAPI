using AccessManagmentAPI.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;


namespace AccessManagmentAPI.Helper
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ContetxtDb _context;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ContetxtDb context): base(options, logger, encoder, clock)
        {
            _context = context;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("No Authorization header found.");
            }

            try
            {
                var headerValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                if (headerValue.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase) && headerValue.Parameter != null)
                {
                    var bytes = Convert.FromBase64String(headerValue.Parameter);
                    string credentials = Encoding.UTF8.GetString(bytes);

                    var parts = credentials.Split(':', 2);
                    if (parts.Length != 2)
                    {
                        return AuthenticateResult.Fail("Invalid Basic authentication header.");
                    }

                    string username = parts[0];
                    string password = parts[1];

                    var user = await _context.TblUsers.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
                    if (user != null)
                    {
                        var claims = new[] { new Claim(ClaimTypes.Name, user.Username) };
                        var identity = new ClaimsIdentity(claims, Scheme.Name);
                        var principal = new ClaimsPrincipal(identity);
                        var ticket = new AuthenticationTicket(principal, Scheme.Name);

                        return AuthenticateResult.Success(ticket);
                    }
                    else
                    {
                        return AuthenticateResult.Fail("Invalid username or password.");
                    }
                }
                else
                {
                    return AuthenticateResult.Fail("Invalid authorization header.");
                }
            }
            catch (FormatException)
            {
                return AuthenticateResult.Fail("Invalid Base64 encoding.");
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Authentication error: {ex.Message}");
            }
        }
    }
}
