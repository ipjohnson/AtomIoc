using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Interfaces
{
    public interface IInterceptor
    {
        int Order { get; }

        bool InsideLifestyle { get; }

        void BeforeConstruction(InjectionContext context);

        void AfterConstruction(InjectionContext context);
    }
}
