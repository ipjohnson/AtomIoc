using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Interfaces;

namespace AtomIoc.Strategies
{
    /// <summary>
    /// Strategy for registering an instance object
    /// </summary>
    public class InstanceStrategy : BasicStrategy
    {
        private readonly object _instance;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="container"></param>
        /// <param name="instance"></param>
        public InstanceStrategy(Container container, object instance) :
            base(container) => _instance = instance;

        /// <summary>
        /// Activation type for strategy
        /// </summary>
        public override Type ActivationType => _instance.GetType();
        
        /// <summary>
        /// Activate strategy
        /// </summary>
        /// <param name="context"></param>
        protected override void ActivateStrategy(InjectionContext context)
        {
            context.Instance = _instance;
        }
    }
}
