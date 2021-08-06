using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Nop.Core.Infrastructure;
using ILogger = Nop.Services.Logging.ILogger;

namespace Nop.Plugin.ExternalAuth.GitHub.GitHubAuthentication
{
    /// <summary>
    /// Authentication handler for GitHub's OAuth based authentication.
    /// </summary>
    public class GitHubHandler : OAuthHandler<GitHubOptions>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="GitHubHandler"/>.
        /// </summary>
        /// <inheritdoc />
        public GitHubHandler(IOptionsMonitor<GitHubOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        { }

        protected virtual ILogger NopLogger => EngineContext.Current.Resolve<ILogger>();

        /// <inheritdoc />
        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            Backchannel.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", tokens.AccessToken);
            var response = await Backchannel.GetAsync(Options.UserInformationEndpoint);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error occurred when retrieving GitHub user information ({response.StatusCode}). Please check if the authentication information is correct and the corresponding GitHub Graph API is enabled.");
            }

            string responseStr = await response.Content.ReadAsStringAsync();
            NopLogger.Information($"GitHub get user info: {JsonConvert.SerializeObject(responseStr)}");

            using (var payload = JsonDocument.Parse(responseStr))
            {
                var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme, Options, Backchannel, tokens, payload.RootElement);
                context.RunClaimActions();
                await Events.CreatingTicket(context);
                return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
            }
        }
    }
}
