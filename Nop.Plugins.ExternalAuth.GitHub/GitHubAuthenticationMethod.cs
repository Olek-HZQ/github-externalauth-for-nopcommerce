using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;

namespace Nop.Plugin.ExternalAuth.GitHub
{
    /// <summary>
    /// Represents method for the authentication with GitHub account
    /// </summary>
    public class GitHubAuthenticationMethod : BasePlugin, IExternalAuthenticationMethod
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public GitHubAuthenticationMethod(ILocalizationService localizationService,
            ISettingService settingService,
            IWebHelper webHelper)
        {
            _localizationService = localizationService;
            _settingService = settingService;
            _webHelper = webHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/GitHubAuthentication/Configure";
        }

        /// <summary>
        /// Gets a name of a view component for displaying plugin in public store
        /// </summary>
        /// <returns>View component name</returns>
        public string GetPublicViewComponentName()
        {
            return GitHubAuthenticationDefaults.VIEW_COMPONENT_NAME;
        }

        /// <summary>
        /// Install the plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task InstallAsync()
        {
            //settings
            await _settingService.SaveSettingAsync(new GitHubExternalAuthSettings());

            //locales
            await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.ExternalAuth.GitHub.ClientKeyIdentifier"] = "client_id",
                ["Plugins.ExternalAuth.GitHub.ClientKeyIdentifier.Hint"] = "Required. The client ID you received from GitHub for your OAuth App.",
                ["Plugins.ExternalAuth.GitHub.ClientSecret"] = "client_secret",
                ["Plugins.ExternalAuth.GitHub.ClientSecret.Hint"] = "Required. The client secret you received from GitHub for your OAuth App.",
                ["Plugins.ExternalAuth.GitHub.Instructions"] = "<p>To configure authentication with GitHub, please follow these steps:<br/><br/><ol><li>Navigate to the <a href=\"https://docs.github.com/en/developers/apps/building-oauth-apps/authorizing-oauth-apps\" target =\"_blank\" > GitHub Docs</a> page and sign in.</li></ol><br/><br/></p>"
            });

            await base.InstallAsync();
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task UninstallAsync()
        {
            //settings
            await _settingService.DeleteSettingAsync<GitHubExternalAuthSettings>();

            //locales
            await _localizationService.DeleteLocaleResourceAsync("Plugins.ExternalAuth.GitHub");

            await base.UninstallAsync();
        }

        #endregion
    }
}