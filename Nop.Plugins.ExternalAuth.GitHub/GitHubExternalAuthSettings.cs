using Nop.Core.Configuration;

namespace Nop.Plugin.ExternalAuth.GitHub
{
    /// <summary>
    /// Represents settings of the GitHub authentication method
    /// </summary>
    public class GitHubExternalAuthSettings : ISettings
    {
        /// <summary>
        /// Gets or sets OAuth2 client identifier
        /// </summary>
        public string ClientKeyIdentifier { get; set; }

        /// <summary>
        /// Gets or sets OAuth2 client secret
        /// </summary>
        public string ClientSecret { get; set; }
    }
}