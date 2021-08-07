using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Nop.Core.Domain.Customers;
using Nop.Services.Authentication.External;
using Nop.Services.Common;
using Nop.Services.Events;

namespace Nop.Plugin.ExternalAuth.GitHub.Infrastructure
{
    /// <summary>
    /// GitHub authentication event consumer (used for saving customer fields on registration)
    /// </summary>
    public class GitHubAuthenticationEventConsumer : IConsumer<CustomerAutoRegisteredByExternalMethodEvent>
    {
        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;

        #endregion

        #region Ctor

        public GitHubAuthenticationEventConsumer(IGenericAttributeService genericAttributeService)
        {
            _genericAttributeService = genericAttributeService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handle event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        public async Task HandleEventAsync(CustomerAutoRegisteredByExternalMethodEvent eventMessage)
        {
            if (eventMessage?.Customer == null || eventMessage.AuthenticationParameters == null)
                return;

            //handle event only for this authentication method
            if (!eventMessage.AuthenticationParameters.ProviderSystemName.Equals(GitHubAuthenticationDefaults.SystemName))
                return;

            //store some of the customer fields
            var name = eventMessage.AuthenticationParameters.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value;
            if (!string.IsNullOrEmpty(name))
                await _genericAttributeService.SaveAttributeAsync(eventMessage.Customer, NopCustomerDefaults.FirstNameAttribute, name);
        }

        #endregion
    }
}