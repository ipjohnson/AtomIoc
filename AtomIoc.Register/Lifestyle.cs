using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Lifestyles;

namespace AtomIoc.Register
{
    public static class Lifestyle
    {
        public static SingletonLifestyle Singleton() => new SingletonLifestyle();
        
        public static ScopedLifestyle Scoped() => new ScopedLifestyle();

        public static NamedScopeLifestyle NamedScope(string scopeName) => new NamedScopeLifestyle(scopeName);
    }
}
