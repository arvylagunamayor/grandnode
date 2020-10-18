using FluentValidation;
using Grand.Domain.Customers;
using Grand.Core.Validators;
using Grand.Services.Localization;
using System.Collections.Generic;
using Grand.Api.Models.Mobile.Customers;

namespace Grand.Api.Validators.Mobile.Customers
{
    public class LoginValidator : BaseGrandValidator<MobileLoginModel>
    {
        public LoginValidator(
            IEnumerable<IValidatorConsumer<MobileLoginModel>> validators,
            ILocalizationService localizationService, CustomerSettings customerSettings)
            : base(validators)
        {
            if (!customerSettings.UsernamesEnabled)
            {
                //login by email
                RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Account.Login.Fields.Email.Required"));
                RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
            }
        }
    }
}