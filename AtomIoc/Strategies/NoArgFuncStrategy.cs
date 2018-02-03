using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AtomIoc.Interfaces;
using AtomIoc.Internal;
using AtomIoc.Metadata;

namespace AtomIoc.Strategies
{
    public class NoArgFuncStrategy : IWrapperStrategy
    {
        private MethodInfo _createFunc =
            typeof(NoArgFuncStrategy).GetTypeInfo().GetDeclaredMethod("CreateFunc");

        /// <summary>
        /// Activation type for strategy
        /// </summary>
        public Type ActivationType => typeof(Func<>);

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
            var locateType = context.RequesType.GenericTypeArguments[0];

            var closedMethod = _createFunc.MakeGenericMethod(locateType);

            return closedMethod.Invoke(null, new object[]{ context });
        }

        private static Func<TResult> CreateFunc<TResult>(InjectionContext context)
        {
            return context.Locate<TResult>;
        }

        /// <summary>
        /// Does this strategy meet conditions to be used
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool MeetsCondition(InjectionContext context) => true;

        public Type GetWrappedType(Type wrappedType)
        {
            return wrappedType.GenericTypeArguments[0];
        }

        public object ActivateWrappedStrategy(InjectionContext context, IWrapperContext wrapperContext)
        {
            return null;
        }
    }
}
