using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace El
{
    internal class LazyImpl<T> : Lazy<T> where T : class
    {
        public LazyImpl(IServiceProvider c) : base(() => ActivatorUtilities.GetServiceOrCreateInstance<T>(c))
        { }
    }
}
