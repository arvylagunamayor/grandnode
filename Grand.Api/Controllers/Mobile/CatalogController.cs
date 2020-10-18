using Grand.Api.Commands.Commands.Catalog;
using Grand.Api.Commands.Models.Mobile.Catalog;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RobustErrorHandler.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grand.Api.Controllers.Mobile
{
    [ApiController]
    [Area("Api")]
    [Route("[area]/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [SwaggerTag(description: "Catalog endpoint")]
    public class CatalogController : Controller
    {
        #region Fields

        private readonly IMediator _mediator;

        #endregion

        #region Ctor

        public CatalogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetCategories")]
        public async Task<ActionResult<IList<CategoryModel>>> GetCategories()
        {
            var result = await _mediator.Send(new GetCategoryCommand());

            return result.ToActionResult();
        }

        [HttpGet("GetProducts/{categoryId}")]
        public async Task<ActionResult<IList<ProductOverviewModel>>> GetCategoryProducts(string categoryId)
        {
            var result = await _mediator.Send(new GetProductsCommand(categoryId));

            return result.ToActionResult();
        }
    }
}