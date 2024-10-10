using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace server.Auth
{
    public class AuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _config;
        private readonly string _secretPhrase = string.Empty;
        public AuthHandler(IConfiguration config, IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _config = config;
            _secretPhrase = _config["SecretPhrase"];
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.NoResult();
            }
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

                if (!authHeaderVal.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
                {
                    return AuthenticateResult.Fail("Invalid Authorization scheme");
                }

                var credentialBytes = Convert.FromBase64String(authHeaderVal.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes);

                if (credentials != _secretPhrase)
                {
                    return AuthenticateResult.Fail("Unauthorized");
                }

                var claims = new[] { new Claim(ClaimTypes.Name, "Admin") };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail("Invalid Authorization header");
            }
        }
    }
}
