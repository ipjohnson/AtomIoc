using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Interfaces;

namespace AtomIoc.Strategies
{
    /// <summary>
    /// Strategy for providing a func
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ContextArgStrategy<T> : BasicStrategy
    {
        private readonly Func<InjectionContext, T> _func;

        public ContextArgStrategy(Container container, Func<InjectionContext, T> func) : base(container)
        {
            _func = func;
        }

        /// <summary>
        /// Activation type
        /// </summary>
        public override Type ActivationType => typeof(T);
        
        protected override void ActivateStrategy(InjectionContext context)
        {
            context.Instance = _func(context);
        }
    }
}
