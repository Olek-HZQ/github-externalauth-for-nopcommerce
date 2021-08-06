using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace Nop.Plugin.ExternalAuth.GitHub.GitHubAuthentication
{
    /// <summary>
    /// Configuration options for <see cref="GitHubOptions"/>.
    /// </summary>
    public class GitHubOptions : OAuthOptions
    {
        /// <summary>
        /// Initializes a new <see cref="GitHubOptions"/>.
        /// </summary>
        public GitHubOptions()
        {
            CallbackPath = new PathString("/signin-github");
            AuthorizationEndpoint = GitHubDefaults.AuthorizationEndpoint;
            TokenEndpoint = GitHubDefaults.TokenEndpoint;
            UserInformationEndpoint = GitHubDefaults.UserInformationEndpoint;
        }
    }
}
