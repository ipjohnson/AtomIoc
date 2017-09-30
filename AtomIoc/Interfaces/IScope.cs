using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Data;

namespace AtomIoc.Interfaces
{
    public interface IScope : IExtraDataContainer, IDisposalScope
    {
        object Locate(Type type, object withKey = null, object extraData = null, bool required = true, Func<IStrategy, InjectionContext, bool> filter = null);
    }
}
