using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Interfaces;

namespace AtomIoc.Lifestyles
{
    public class NamedScopeLifestyle : ILifestyle
    {
        protected readonly string ScopeName;
        protected readonly string UniqueId = Utilities.UniqueId.Generate();

        public NamedScopeLifestyle(string scopeName)
        {
            ScopeName = scopeName;
        }

        public void GetValue(InjectionContext context, Container container, Action<InjectionContext> createValue)
        {
            var namedScope = context.Container;

            while (namedScope != null && namedScope.Name != ScopeName)
            {
                namedScope = namedScope.Parent;
            }

            if (namedScope == null)
            {
                throw new Exception($"Could not locate scope by the name of {ScopeName}");
            }
            
            var value = namedScope.GetExtraData(UniqueId);

            if (value != null)
            {
                context.Instance = value;
                return;
            }

            createValue(context);

            // use the one set just incase another thread got there first
            context.Instance = namedScope.SetExtraData(UniqueId, context.Instance);
        }

        public ILifestyle Clone()
        {
            return new NamedScopeLifestyle(ScopeName);
        }
    }
}
