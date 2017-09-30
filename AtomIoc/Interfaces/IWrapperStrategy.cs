using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Interfaces
{
    public interface IWrapperStrategy : IStrategy
    {
        Type GetWrappedType(Type wrappedType);


    }
}
