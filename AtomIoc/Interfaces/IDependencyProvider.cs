using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Interfaces
{
    public enum DependencyType
    {
        ConstructorParameter = 1,
        Property = 2,
        MethodParameter = 4
    }

    public interface IDependencyProvider
    {
        DependencyType DependencyType { get; }

        string Name { get; }

        Type Type { get; }

        object ProvideValue(InjectionContext context);
    }
}
