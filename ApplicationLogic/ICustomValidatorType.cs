namespace ApplicationLogic
{
    public interface ICustomValidatorType<T>
    {
        bool IsValid(T request);
    }
}
