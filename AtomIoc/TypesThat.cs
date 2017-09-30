using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Utilities;

namespace AtomIoc
{
    public static class TypesThat
    {
        public static TypesThatFilter HaveAttribute<T>(Func<T, bool> filter) where T : Attribute
        {
            return new TypesThatFilter().HaveAttribute(filter);
        }
    }
}
