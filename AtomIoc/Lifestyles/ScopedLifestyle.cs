using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Interfaces;

namespace AtomIoc.Lifestyles
{
    public class ScopedLifestyle : ILifestyle
    {
        protected string UniqueId = Utilities.UniqueId.Generate();

        public void GetValue(InjectionContext context, Container container, bool externallyOwned, Action<InjectionContext> createValue)
        {
            var value = context.Container.GetExtraData(UniqueId);

            if (value != null)
            {
                context.Instance = value;
                return;
            }

            createValue(context);

            // use the one set just incase another thread got there first
            context.Instance = context.Container.SetExtraData(UniqueId, context.Instance);

            if (!externallyOwned && context.Instance is IDisposable)
            {
                context.Container.AddDisposable(context.Instance);
            }
        }

        public ILifestyle Clone()
        {
            return new ScopedLifestyle();
        }
    }
}
