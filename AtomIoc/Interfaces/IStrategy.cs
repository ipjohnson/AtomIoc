using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Interfaces
{
    /// <summary>
    /// Interface for defining a strategy
    /// </summary>
    public interface IStrategy
    {
        /// <summary>
        /// Activation type for strategy
        /// </summary>
        Type ActivationType { get; }

        /// <summary>
        /// Lifestyle for strategy
        /// </summary>
        ILifestyle Lifestyle { get; }

        /// <summary>
        /// Activate the strategy
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        object Activate(InjectionContext context);

        /// <summary>
        /// Does this strategy meet conditions to be used
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool MeetsCondition(InjectionContext context);
    }
}
