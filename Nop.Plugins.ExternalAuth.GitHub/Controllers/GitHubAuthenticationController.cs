using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nop.Core;
using Nop.Plugin.ExternalAuth.GitHub.GitHubAuthentication;
using Nop.Plugin.ExternalAuth.GitHub.Models;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.ExternalAuth.GitHub.Controllers
{
    public class GitHubAuthenticationController : BasePluginController
    {
        #region Fields

        private readonly GitHubExternalAuthSettings _gitHubExternalAuthSettings;
        private readonly IAuthenticationPluginManager _authenticationPluginManager;
        private readonly IExternalAuthenticationService _externalAuthenticationService;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IOptionsMonitorCache<GitHubOptions> _optionsCache;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public GitHubAuthenticationController(GitHubExternalAuthSettings GitHubExternalAuthSettings,
            IAuthenticationPluginManager authenticationPluginManager,
            IExternalAuthenticationService externalAuthenticationService,
            ILocalizationService localizationService,
            INotificationService notificationService,
            IOptionsMonitorCache<GitHubOptions> optionsCache,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext,
            IWorkContext workContext)
        {
            _gitHubExternalAuthSettings = GitHubExternalAuthSettings;
            _authenticationPluginManager = authenticationPluginManager;
            _externalAuthenticationService = externalAuthenticationService;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _optionsCache = optionsCache;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
            _workContext = workContext;
        }

        #endregion

        #region Methods

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedView();

            var model = new ConfigurationModel
            {
                ClientId = _gitHubExternalAuthSettings.ClientKeyIdentifier,
                ClientSecret = _gitHubExternalAuthSettings.ClientSecret
            };

            return View("~/Plugins/ExternalAuth.GitHub/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return Configure();

            //save settings
            _gitHubExternalAuthSettings.ClientKeyIdentifier = model.ClientId;
            _gitHubExternalAuthSettings.ClientSecret = model.ClientSecret;
            _settingService.SaveSetting(_gitHubExternalAuthSettings);

            //clear GitHub authentication options cache
            _optionsCache.TryRemove(GitHubDefaults.AuthenticationScheme);

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        public IActionResult Login(string returnUrl)
        {
            var methodIsAvailable = _authenticationPluginManager
                .IsPluginActive(GitHubAuthenticationDefaults.SystemName, _workContext.CurrentCustomer, (_storeContext.CurrentStore).Id);
            if (!methodIsAvailable)
                throw new NopException("GitHub authentication module cannot be loaded");

            if (string.IsNullOrEmpty(_gitHubExternalAuthSettings.ClientKeyIdentifier) ||
                string.IsNullOrEmpty(_gitHubExternalAuthSettings.ClientSecret))
            {
                throw new NopException("GitHub authentication module not configured");
            }

            //configure login callback action
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("LoginCallback", "GitHubAuthentication", new { returnUrl })
            };
            authenticationProperties.SetString(GitHubAuthenticationDefaults.ErrorCallback, Url.RouteUrl("Login", new { returnUrl }));

            return Challenge(authenticationProperties, GitHubDefaults.AuthenticationScheme);
        }

        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<IActionResult> LoginCallback(string returnUrl)
        {
            //authenticate GitHub user
            var authenticateResult = await HttpContext.AuthenticateAsync(GitHubDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded || !authenticateResult.Principal.Claims.Any())
                return RedirectToRoute("Login");

            //create external authentication parameters
            var authenticationParameters = new ExternalAuthenticationParameters
            {
                ProviderSystemName = GitHubAuthenticationDefaults.SystemName,
                AccessToken = await HttpContext.GetTokenAsync(GitHubDefaults.AuthenticationScheme, "access_token"),
                Email = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email)?.Value,
                ExternalIdentifier = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value,
                ExternalDisplayIdentifier = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Name)?.Value,
                Claims = authenticateResult.Principal.Claims.Select(claim => new ExternalAuthenticationClaim(claim.Type, claim.Value)).ToList()
            };

            //authenticate Nop user
            return _externalAuthenticationService.Authenticate(authenticationParameters, returnUrl);
        }

        #endregion
    }
}