using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Interfaces
{
    public interface IInterceptorProvider
    {
        IInterceptor ProvideInterceptor(IStrategy strategy);
    }
}
