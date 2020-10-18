using Grand.Api.Jwt;
using MediatR;
using RobustErrorHandler.Core;
using RobustErrorHandler.Core.Errors;
using RobustErrorHandler.Core.SuccessCollection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grand.Api.Commands.Commands
{
    public class LoginCommand : IRequest<Either<Error, Success<JwtToken>>>
    {

        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; }
        public string Password { get; }
    }
}
