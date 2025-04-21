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
using robot_controller_api.Persistence; // Important for UserDataAccess
using Microsoft.EntityFrameworkCore; // Important for EF queries
namespace robot_controller_api.Authentication;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly UserDataAccess _db;

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        UserDataAccess dbContext) // Inject your context here
        : base(options, logger, encoder)
    {
        _db = dbContext;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var endpoint = Context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
        {
            return AuthenticateResult.NoResult();
        }

        Response.Headers.Append("WWW-Authenticate", @"Basic realm=""Access to the robot controller.""");

        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("Missing Authorization Header");
        }

        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? string.Empty);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);

            if (credentials.Length != 2)
                return AuthenticateResult.Fail("Invalid Authorization Header");

            var username = credentials[0];
            var password = credentials[1];

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == username);
            if (user == null)
                return AuthenticateResult.Fail("Invalid Username");

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return AuthenticateResult.Fail("Invalid Password");

            var claims = new[] {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Authentication failed.");
            return AuthenticateResult.Fail("Authentication failed.");
        }
    }
}
