using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Interfaces;

namespace AtomIoc.Internal
{
    internal static class InternalLocateExtensions
    {
        public static T Locate<T>(this InjectionContext context)
        {
            return (T) ((IScope)context.Container).Locate(typeof(T));
        }
    }
}
