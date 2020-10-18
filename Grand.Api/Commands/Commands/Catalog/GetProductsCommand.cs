using Grand.Api.Commands.Models.Mobile.Catalog;
using MediatR;
using RobustErrorHandler.Core;
using RobustErrorHandler.Core.Errors;
using RobustErrorHandler.Core.SuccessCollection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grand.Api.Commands.Commands.Catalog
{
    public class GetProductsCommand : IRequest<Either<Error, Success<IList<ProductOverviewModel>>>>
    {
        public string CategoryId { get; }

        public GetProductsCommand(string categoryId)
        {
            CategoryId = categoryId;
        }
    }
}
