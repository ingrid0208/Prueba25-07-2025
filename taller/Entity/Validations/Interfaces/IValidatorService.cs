namespace Entity.Validations.Interfaces
{
    public interface IValidatorService
    {
        Task ValidateAsync<T>(T instance, CancellationToken ct = default);
    }
}
