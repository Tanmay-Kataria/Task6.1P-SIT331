using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.Extensions.Internal;

namespace robot_controller_api.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            TimeClock timeProvider)
            : base(options, logger, encoder, timeProvider)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Check if the endpoint has [AllowAnonymous]
            var endpoint = Context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            // Append Basic Auth header for unauthorized responses.
            Response.Headers.Append("WWW-Authenticate", @"Basic realm=""Access to the robot controller.""");

            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
            }

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? string.Empty);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);

                if (credentials.Length != 2)
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
                }

                var username = credentials[0];
                var password = credentials[1];

                // Here you would typically load the user from your DB.
                // For demonstration purposes, we'll use a dummy user.
                // Assume a user with username "admin" exists.
                if (username != "admin")
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Username"));
                }

                // Retrieve the hashed password from your datastore.
                // For demonstration, we assume the hash is generated via BCrypt.
                // Example: var storedHash = "<stored hash from DB>";
                // In this example, we hash "sit331password" to simulate a stored hash.
                var storedHash = BCrypt.Net.BCrypt.HashPassword("sit331password");

                // Verify the provided password against the stored hash.
                bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, storedHash);
                if (!isValidPassword)
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Password"));
                }

                // Create claims and authentication ticket.
                var claims = new[] {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "RobotControllerUser")
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }
        }
    }
}
