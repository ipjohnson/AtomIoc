using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AtomIoc.Interfaces;

namespace AtomIoc.Register
{
    public static class InitializeExtensions
    {
        public static Container Initialize<T>(this Container container, Action<T> initialize)
        {
            container.AddInspector(new InitializeInspector<T>(initialize));

            return container;
        }

        public class InitializeInspector<T> : IInterceptorProvider
        {
            private readonly Action<T> _initialize;

            public InitializeInspector(Action<T> initialize)
            {
                _initialize = initialize;
            }

            public IInterceptor ProvideInterceptor(IStrategy strategy)
            {
                return typeof(T).GetTypeInfo().IsAssignableFrom(strategy.ActivationType.GetTypeInfo()) ? 
                    new InitializeInterceptor<T>(_initialize) : 
                    null;
            }
        }

        public class InitializeInterceptor<T> : IInterceptor
        {
            private readonly Action<T> _initialize;

            public InitializeInterceptor(Action<T> initialize)
            {
                _initialize = initialize;
            }

            public int Order => 0;

            public bool InsideLifestyle => true;

            public void BeforeConstruction(InjectionContext context)
            {

            }

            public void AfterConstruction(InjectionContext context)
            {
                _initialize((T) context.Instance);
            }
        }
    }

}
