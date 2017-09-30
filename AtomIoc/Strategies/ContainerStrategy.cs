using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Interfaces;

namespace AtomIoc.Strategies
{
    /// <summary>
    /// Container strategy
    /// </summary>
    public class ContainerStrategy : IStrategy
    {
        /// <summary>
        /// Activation type for strategy
        /// </summary>
        public Type ActivationType => typeof(Container);

        /// <summary>
        /// Lifestyle for strategy
        /// </summary>
        public ILifestyle Lifestyle => null;

        /// <summary>
        /// Activate the strategy
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public object Activate(InjectionContext context)
        {
            return context.Container;
        }

        /// <summary>
        /// Does this strategy meet conditions to be used
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool MeetsCondition(InjectionContext context)
        {
            return true;
        }
    }
}
