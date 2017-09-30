using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Interfaces;

namespace AtomIoc
{
    public static class ResolveExtensions
    {
        public static T Resolve<T>(this IScope scope, object withKey = null, object extraData = null, bool required = true,
            Func<IStrategy, InjectionContext, bool> filter = null)
        {
            return (T)scope.Locate(typeof(T), withKey, extraData, required, filter);
        }

        public static object Resolve(this IScope scope, Type type, object withKey = null, object extraData = null, bool required = true,
            Func<IStrategy, InjectionContext, bool> filter = null)
        {
            return scope.Locate(type, withKey, extraData, required, filter);
        }
    }
}
