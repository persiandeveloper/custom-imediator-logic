using MediatR;

namespace ApplicationLogic
{
    public interface ICustomValidator
    {
        bool IsValid();
    }
}
