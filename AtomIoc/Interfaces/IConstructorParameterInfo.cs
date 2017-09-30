using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AtomIoc.Interfaces
{
    public interface IConstructorParameterInfo
    {
        bool Match(ParameterInfo parameter);

        object ProvideValue(InjectionContext context);
    }
}
