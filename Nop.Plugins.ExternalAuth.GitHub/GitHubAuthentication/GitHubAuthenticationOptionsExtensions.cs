using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Nop.Plugin.ExternalAuth.GitHub.GitHubAuthentication
{
    /// <summary>
    /// Extension methods to configure GitHub OAuth authentication.
    /// </summary>
    public static class GitHubAuthenticationOptionsExtensions
    {
        /// <summary>
        /// Adds GitHub OAuth-based authentication to <see cref="AuthenticationBuilder"/> using the default scheme.
        /// The default scheme is specified by <see cref="GitHubDefaults.AuthenticationScheme"/>.
        /// <para>
        /// GitHub authentication allows application users to sign in with their GitHub account.
        /// </para>
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
        /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
        public static AuthenticationBuilder AddGitHub(this AuthenticationBuilder builder)
            => builder.AddGitHub(GitHubDefaults.AuthenticationScheme, _ => { });

        /// <summary>
        /// Adds GitHub OAuth-based authentication to <see cref="AuthenticationBuilder"/> using the default scheme.
        /// The default scheme is specified by <see cref="GitHubDefaults.AuthenticationScheme"/>.
        /// <para>
        /// GitHub authentication allows application users to sign in with their GitHub account.
        /// </para>
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
        /// <param name="configureOptions">A delegate to configure <see cref="GitHubOptions"/>.</param>
        /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
        public static AuthenticationBuilder AddGitHub(this AuthenticationBuilder builder, Action<GitHubOptions> configureOptions)
            => builder.AddGitHub(GitHubDefaults.AuthenticationScheme, configureOptions);

        /// <summary>
        /// Adds GitHub OAuth-based authentication to <see cref="AuthenticationBuilder"/> using the default scheme.
        /// The default scheme is specified by <see cref="GitHubDefaults.AuthenticationScheme"/>.
        /// <para>
        /// GitHub authentication allows application users to sign in with their GitHub account.
        /// </para>
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
        /// <param name="authenticationScheme">The authentication scheme.</param>
        /// <param name="configureOptions">A delegate to configure <see cref="GitHubOptions"/>.</param>
        /// <returns>A reference to <paramref name="builder"/> after the operation has completed.</returns>
        public static AuthenticationBuilder AddGitHub(this AuthenticationBuilder builder, string authenticationScheme, Action<GitHubOptions> configureOptions)
            => builder.AddGitHub(authenticationScheme, GitHubDefaults.DisplayName, configureOptions);

        /// <summary>
        /// Adds GitHub OAuth-based authentication to <see cref="AuthenticationBuilder"/> using the default scheme.
        /// The default scheme is specified by <see cref="GitHubDefaults.AuthenticationScheme"/>.
        /// <para>
        /// GitHub authentication allows application users to sign in with their GitHub account.
        /// </para>
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
        /// <param name="authenticationScheme">The authentication scheme.</param>
        /// <param name="displayName">A display name for the authentication handler.</param>
        /// <param name="configureOptions">A delegate to configure <see cref="GitHubOptions"/>.</param>
        public static AuthenticationBuilder AddGitHub(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<GitHubOptions> configureOptions)
            => builder.AddOAuth<GitHubOptions, GitHubHandler>(authenticationScheme, displayName, configureOptions);
    }
}
