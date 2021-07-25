using MediatR;
using System.Threading.Tasks;

namespace ApplicationLogic
{
    public class CreateUserCommandValidator : ICustomValidatorType<CreateUserCommand>
    {

        public Task<bool> IsValid(CreateUserCommand request)
        {
            return Task.FromResult(true);
        }
    }
}
