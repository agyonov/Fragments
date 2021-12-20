using Polly;
using Polly.Retry;

namespace El
{
    public class RetryService : Interfaces.IRetryService
    {
        private readonly AsyncRetryPolicy policyAsync;

        public RetryService()
        {
            policyAsync = Policy.Handle<Exception>()
                                .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromMilliseconds(retryAttempt * 250));
        }

        public async Task<T> ExecuteWithRetry<T>(Func<Task<T>> method) where T : notnull
        {

            T res = default!;
            var resP = await policyAsync
              .ExecuteAndCaptureAsync(async () =>
              {
                  res = await method();
              })
              .ConfigureAwait(false);

            if (resP.FinalException != null) {
                throw resP.FinalException;
            }

            return res!;
        }

        public async Task<T> ExecuteWithRetry<T>(Func<CancellationToken, Task<T>> method, CancellationToken ct) where T : notnull
        {

            T res = default!;
            var resP = await policyAsync
              .ExecuteAndCaptureAsync(async (ct) =>
              {
                  res = await method(ct);
              }, ct)
              .ConfigureAwait(false);

            if (resP.FinalException != null) {
                throw resP.FinalException;
            }

            return res!;
        }


        public async Task<T> ExecuteWithParamRetry<T, M>(Func<M, Task<T>> method, M data) where T : notnull
        {

            T res = default!;
            var resP = await policyAsync
              .ExecuteAndCaptureAsync(async () =>
              {
                  res = await method(data);
              })
              .ConfigureAwait(false);

            if (resP.FinalException != null) {
                throw resP.FinalException;
            }

            return res!;
        }

        public async Task<T> ExecuteWithParamRetry<T, M>(Func<M, CancellationToken, Task<T>> method, M data, CancellationToken ct) where T : notnull
        {

            T res = default!;
            var resP = await policyAsync
              .ExecuteAndCaptureAsync(async (ct) =>
              {
                  res = await method(data, ct);
              }, ct)
              .ConfigureAwait(false);

            if (resP.FinalException != null) {
                throw resP.FinalException;
            }

            return res!;
        }
    }
}
