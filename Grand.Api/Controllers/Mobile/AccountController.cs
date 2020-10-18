using Grand.Api.Commands.Commands;
using Grand.Api.Jwt;
using Grand.Api.Models.Mobile.Customers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RobustErrorHandler.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace Grand.Api.Controllers.Mobile
{
    [ApiController]
    [Area("Api")]
    [Route("[area]/[controller]")]
    [ApiExplorerSettings(IgnoreApi = false)]
    [SwaggerTag(description: "Login customer")]
    public class AccountController : Controller
    {
        #region Fields

        private readonly IMediator _mediator;

        #endregion

        #region Ctor

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #endregion

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("Login")]
        public async Task<ActionResult<JwtToken>> Login([FromBody] MobileLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var base64EncodedBytes = System.Convert.FromBase64String(model.Password);
                var password = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                var command = new LoginCommand(model.Email, password);

                var result = await _mediator.Send(command);

                return result.ToActionResult();
            }

            return BadRequest(ModelState.Root.Errors[0].ErrorMessage);
        }
    }
}