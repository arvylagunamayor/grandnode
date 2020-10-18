using MediatR;
using RobustErrorHandler.Core;
using RobustErrorHandler.Core.Errors;
using RobustErrorHandler.Core.SuccessCollection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grand.Api.Commands.Models.Mobile.Catalog
{
    public class GetCategoryCommand : IRequest<Either<Error, Success<IList<CategoryModel>>>>
    {
        public GetCategoryCommand() { }
    }
}
