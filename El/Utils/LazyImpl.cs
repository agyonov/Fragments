using Microsoft.Extensions.DependencyInjection;

namespace El
{
    internal class LazyImpl<T> : Lazy<T> where T : class
    {
        public LazyImpl(IServiceProvider c) : base(() => ActivatorUtilities.GetServiceOrCreateInstance<T>(c))
        { }
    }
}
