using MediatR;
using System;

namespace ApplicationLogic
{
    public class CreateUserCommand : IRequest<string>
    {
        public string UserName { get; set; }

        public CreateUserCommand(string userName)
        {
            UserName = userName;
        }
    }
}
