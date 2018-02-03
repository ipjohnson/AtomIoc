using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AtomIoc.Interfaces;

namespace AtomIoc.Register
{
    public static class InitializeExtensions
    {
        /// <summary>
        /// Initialize specific object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <param name="initialize"></param>
        /// <param name="insideLifestyle"></param>
        /// <returns></returns>
        public static Container Initialize<T>(this Container container, Action<T> initialize, bool insideLifestyle = true)
        {
            container.AddInspector(new InitializeInspector<T>(initialize, insideLifestyle));

            return container;
        }

        public class InitializeInspector<T> : IInterceptorProvider
        {
            private readonly Action<T> _initialize;
            private readonly bool _insideLifestyle;

            public InitializeInspector(Action<T> initialize, bool insideLifestyle)
            {
                _initialize = initialize;
                _insideLifestyle = insideLifestyle;
            }

            public IInterceptor ProvideInterceptor(IStrategy strategy)
            {
                return typeof(T).GetTypeInfo().IsAssignableFrom(strategy.ActivationType.GetTypeInfo()) ? 
                    new InitializeInterceptor<T>(_initialize, _insideLifestyle) : 
                    null;
            }
        }

        public class InitializeInterceptor<T> : IInterceptor
        {
            private readonly Action<T> _initialize;

            public InitializeInterceptor(Action<T> initialize, bool insideLifestyle)
            {
                _initialize = initialize;
                InsideLifestyle = insideLifestyle;
            }

            public int Order => 0;

            public bool InsideLifestyle { get; }

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
