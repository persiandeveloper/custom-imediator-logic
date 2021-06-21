using MediatR;

namespace ApplicationLogic
{
    public class CreateUserCommandValidator : ICustomValidatorType<CreateUserCommand>
    {
        public bool IsValid(CreateUserCommand request)
        {
            return true;
        }
    }
}
