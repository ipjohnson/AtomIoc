using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AtomIoc.Interfaces;
using AtomIoc.Metadata;

namespace AtomIoc.Strategies
{
    public class EnumerableStrategy : IStrategy
    {
        private MethodInfo _yieldMethodInfo;

        public EnumerableStrategy()
        {
            _yieldMethodInfo = GetType().GetTypeInfo().GetDeclaredMethod("YieldStrategies");
        }

        /// <summary>
        /// Activation type for strategy
        /// </summary>
        public Type ActivationType => typeof(IEnumerable<>);

        /// <summary>
        /// Metadata for strategy
        /// </summary>
        public IMetadata Metadata => EmptyMetadata.Instance;
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
            var wrappedType = context.RequesType.GenericTypeArguments[0];

            var newContext = context.Child(wrappedType, null);

            var strategies = context.Container.FindAllStrategies(newContext, null);

            return _yieldMethodInfo.MakeGenericMethod(wrappedType).Invoke(this, new object[] { strategies, context });
        }

        private IEnumerable<T> YieldStrategies<T>(IEnumerable<IStrategy> strategies, InjectionContext context)
        {
            foreach (var strategy in strategies)
            {
                var newContext = context.Child(typeof(T), null);

                if (strategy is IWrapperStrategy)
                {

                }
                else
                {
                    yield return (T) strategy.Activate(newContext);
                }
            }
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
