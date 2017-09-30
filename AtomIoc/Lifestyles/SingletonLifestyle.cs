using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Interfaces;

namespace AtomIoc.Lifestyles
{
    public class SingletonLifestyle : ILifestyle
    {
        private volatile object _singleton;
        private readonly object _lockObject = new object();

        public void GetValue(InjectionContext context, Container container, Action<InjectionContext> createValue)
        {
            if (_singleton != null)
            {
                context.Instance = _singleton;
                return;
            }

            lock (_lockObject)
            {
                if (_singleton == null)
                {
                    var currentContext = context.Container.Configuration.SingletonCreatesNewContext
                        ? new InjectionContext(context.RequesType, context.Key, container, null)
                        : context;

                    createValue(currentContext);

                    _singleton = currentContext.Instance;

                    if (currentContext != context)
                    {
                        context.Instance = currentContext.Instance;
                    }
                }
                else
                {
                    context.Instance = _singleton;
                }
            }
        }

        public ILifestyle Clone()
        {
            return new SingletonLifestyle();
        }
    }
}
