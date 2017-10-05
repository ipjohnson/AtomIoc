using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Interfaces;
using AtomIoc.Metadata;

namespace AtomIoc.Strategies
{
    public class MetaStrategy : IWrapperStrategy
    {
        /// <summary>
        /// Activation type for strategy
        /// </summary>
        public Type ActivationType => typeof(Meta<>);

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
            var type = context.RequesType.GenericTypeArguments[0];
            var childContext = context.Child(type, context.Key, context.MemberInjection);

            var strategy = context.Container.FindStrategy(childContext);

            if (strategy == null)
            {
                if (context.IsRequired)
                {
                    throw new Exception($"Could not locate strategy for type {type.FullName}");
                }

                return null;
            }

            var value = strategy.Activate(childContext);

            return Activator.CreateInstance(context.RequesType, value, strategy.Metadata);
        }

        /// <summary>
        /// Does this strategy meet conditions to be used
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool MeetsCondition(InjectionContext context) => true;

        public Type GetWrappedType(Type wrappedType)
        {
            throw new NotImplementedException();
        }

        public object ActivateWrappedStrategy(InjectionContext context, IWrapperContext wrapperContext)
        {
            throw new NotImplementedException();
        }
    }
}
