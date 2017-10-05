using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Interfaces
{
    public interface IWrapperContext
    {
        object ActivateObject(InjectionContext context);

        IStrategy WrappedStrategy { get; set; }
    }
    
    public interface IWrapperStrategy : IStrategy
    {
        Type GetWrappedType(Type wrappedType);

        object ActivateWrappedStrategy(InjectionContext context, IWrapperContext wrapperContext);
    }
}
