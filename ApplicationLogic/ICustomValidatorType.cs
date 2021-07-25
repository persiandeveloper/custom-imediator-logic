using System.Threading.Tasks;

namespace ApplicationLogic
{
    public interface ICustomValidatorType<T>
    {
        Task<bool> IsValid(T request);
    }
}
