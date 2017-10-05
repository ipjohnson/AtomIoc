using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Interfaces;

namespace AtomIoc.Strategies
{

    public class WrapperContext : IWrapperContext
    {
        public object ActivateObject(InjectionContext context)
        {
            return null;
        }

        public IStrategy WrappedStrategy { get; set; }
    }
}
