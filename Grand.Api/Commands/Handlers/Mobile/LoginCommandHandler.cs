using Grand.Api.Commands.Commands;
using Grand.Api.Commands.Models.Common;
using Grand.Api.Jwt;
using Grand.Core;
using Grand.Domain.Customers;
using Grand.Services.Authentication;
using Grand.Services.Common;
using Grand.Services.Customers;
using Grand.Services.Localization;
using Grand.Services.Notifications.Customers;
using MediatR;
using RobustErrorHandler.Core;
using RobustErrorHandler.Core.Errors;
using RobustErrorHandler.Core.SuccessCollection;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Grand.Api.Commands.Handlers.Mobile
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Either<Error, Success<JwtToken>>>
    {
        #region Fields

        private readonly IGrandAuthenticationService _authenticationService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly IMediator _mediator;
        private readonly CustomerSettings _customerSettings;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor

        public LoginCommandHandler(
            IGrandAuthenticationService authenticationService,
            IWorkContext workContext,
            IStoreContext storeContext,
            ICustomerService customerService,
            IGenericAttributeService genericAttributeService,
            ICustomerRegistrationService customerRegistrationService,
            ILocalizationService localizationService,
            IMediator mediator,
            CustomerSettings customerSettings)
        {
            _authenticationService = authenticationService;
            _workContext = workContext;
            _storeContext = storeContext;
            _customerService = customerService;
            _genericAttributeService = genericAttributeService;
            _customerRegistrationService = customerRegistrationService;
            _localizationService = localizationService;
            _customerSettings = customerSettings;
            _mediator = mediator;
        }

        #endregion

        public async Task<Either<Error, Success<JwtToken>>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loginResult = await _customerRegistrationService.ValidateCustomer(request.Email, request.Password);

            switch (loginResult)
            {
                case CustomerLoginResults.Successful:
                    {
                        var customer = await _customerService.GetCustomerByEmail(request.Email);

                        var claims = new Dictionary<string, string>();
                        claims.Add("Email", customer.Email);

                        var token = await _mediator.Send(new GenerateTokenCommand() { Claims = claims });

                        //raise event       
                        await _mediator.Publish(new CustomerLoggedInEvent(customer));

                        return Result.Success(token);
                    }
                case CustomerLoginResults.RequiresTwoFactor:
                    {
                        //var userName = _customerSettings.UsernamesEnabled ? model.Username : model.Email;

                        //HttpContext.Session.SetString("RequiresTwoFactor", userName);

                        return Result.Unauthorized<Success<JwtToken>>(new DefaultMessage("RequiresTwoFactor"));
                    }

                case CustomerLoginResults.CustomerNotExist:
                    return Result.Unauthorized<Success<JwtToken>>(new DefaultMessage(_localizationService.GetResource("Account.Login.WrongCredentials.CustomerNotExist")));
                case CustomerLoginResults.Deleted:
                    return Result.Unauthorized<Success<JwtToken>>(new DefaultMessage(_localizationService.GetResource("Account.Login.WrongCredentials.Deleted")));
                case CustomerLoginResults.NotActive:
                    return Result.Unauthorized<Success<JwtToken>>(new DefaultMessage(_localizationService.GetResource("Account.Login.WrongCredentials.NotActive")));
                case CustomerLoginResults.NotRegistered:
                    return Result.Unauthorized<Success<JwtToken>>(new DefaultMessage(_localizationService.GetResource("Account.Login.WrongCredentials.NotRegistered")));
                case CustomerLoginResults.LockedOut:
                    return Result.Unauthorized<Success<JwtToken>>(new DefaultMessage(_localizationService.GetResource("Account.Login.WrongCredentials.LockedOut")));
                case CustomerLoginResults.WrongPassword:
                    return Result.Unauthorized<Success<JwtToken>>(new DefaultMessage(_localizationService.GetResource("Account.Login.WrongCredentials")));
                default:
                    return Result.Unauthorized<Success<JwtToken>>(new DefaultMessage(_localizationService.GetResource("Account.Login.WrongCredentials")));
            }
        }
    }
}
