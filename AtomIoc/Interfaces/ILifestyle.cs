using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Interfaces
{
    public interface ILifestyle
    {
        void GetValue(InjectionContext context, Container container, bool externallyOwned, Action<InjectionContext> createValue);

        ILifestyle Clone();
    }
}
