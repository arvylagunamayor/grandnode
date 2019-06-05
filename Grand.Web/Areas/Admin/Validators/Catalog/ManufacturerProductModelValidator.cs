﻿using FluentValidation;
using Grand.Core;
using Grand.Core.Domain.Customers;
using Grand.Framework.Validators;
using Grand.Services.Catalog;
using Grand.Services.Localization;
using Grand.Web.Areas.Admin.Extensions;
using Grand.Web.Areas.Admin.Models.Catalog;
using System.Linq;

namespace Grand.Web.Areas.Admin.Validators.Catalog
{
    public class ManufacturerProductModelValidator : BaseGrandValidator<ManufacturerModel.ManufacturerProductModel>
    {
        public ManufacturerProductModelValidator(ILocalizationService localizationService, IManufacturerService manufacturerService, IWorkContext workContext)
        {
            if (workContext.CurrentCustomer.IsStaff())
            {
                RuleFor(x => x).MustAsync(async (x, y, context) =>
                {
                    var manufacturer = await manufacturerService.GetManufacturerById(x.ManufacturerId);
                    if (manufacturer != null)
                        if (!manufacturer.AccessToEntityByStore(workContext.CurrentCustomer.StaffStoreId))
                            return false;

                    return true;
                }).WithMessage(localizationService.GetResource("Admin.Catalog.Manufacturers.Permisions"));
            }
        }
    }
}