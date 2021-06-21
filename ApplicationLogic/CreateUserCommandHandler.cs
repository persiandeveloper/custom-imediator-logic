using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLogic
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return string.Empty;
        }
    }
}
