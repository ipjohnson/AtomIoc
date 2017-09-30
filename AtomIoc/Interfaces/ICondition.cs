using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Interfaces
{
    public interface ICondition
    {
        bool MeetsCondition(IStrategy strategy, InjectionContext context);
    }
}
