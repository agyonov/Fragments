namespace El.Interfaces
{
    public interface IRetryService
    {
        Task<T> ExecuteWithRetry<T>(Func<Task<T>> method);
        Task<T> ExecuteWithRetry<T>(Func<CancellationToken, Task<T>> method, CancellationToken ct);
        Task<T> ExecuteWithParamRetry<T, M>(Func<M, Task<T>> method, M data);
        Task<T> ExecuteWithParamRetry<T, M>(Func<M, CancellationToken, Task<T>> method, M data, CancellationToken ct);
    }
}
