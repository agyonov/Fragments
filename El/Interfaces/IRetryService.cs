namespace El.Interfaces
{
    public interface IRetryService
    {
        Task<T> ExecuteWithRetry<T>(Func<Task<T>> method) where T : notnull;
        Task<T> ExecuteWithRetry<T>(Func<CancellationToken, Task<T>> method, CancellationToken ct) where T : notnull;
        Task<T> ExecuteWithParamRetry<T, M>(Func<M, Task<T>> method, M data) where T : notnull;
        Task<T> ExecuteWithParamRetry<T, M>(Func<M, CancellationToken, Task<T>> method, M data, CancellationToken ct) where T : notnull;
    }
}
