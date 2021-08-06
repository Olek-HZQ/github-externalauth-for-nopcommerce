namespace Nop.Plugin.ExternalAuth.GitHub.GitHubAuthentication
{
    /// <summary>
    /// Default values for the GitHub authentication handler.
    /// </summary>
    public class GitHubDefaults
    {
        /// <summary>
        /// The default scheme for GitHub authentication. The value is <c>GitHub</c>.
        /// </summary>
        public const string AuthenticationScheme = "GitHub";

        /// <summary>
        /// The default display name for GitHub authentication. Defaults to <c>GitHub</c>.
        /// </summary>
        public static readonly string DisplayName = "GitHub";

        /// <summary>
        /// The default endpoint used to perform GitHub authentication.
        /// </summary>
        public static readonly string AuthorizationEndpoint = "https://github.com/login/oauth/authorize";

        /// <summary>
        /// The OAuth endpoint used to retrieve access tokens.
        /// </summary>
        public static readonly string TokenEndpoint = "https://github.com/login/oauth/access_token";

        /// <summary>
        /// The GitHub Graph API endpoint that is used to gather additional user information.
        /// </summary>
        public static readonly string UserInformationEndpoint = "https://api.github.com/user";
    }
}
