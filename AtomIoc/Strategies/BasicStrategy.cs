using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AtomIoc.Data;
using AtomIoc.Interfaces;
using AtomIoc.Metadata;

namespace AtomIoc.Strategies
{
    public abstract class BasicStrategy : IConfigurableStrategy
    {
        protected static readonly IInterceptor[] EmptyInterceptors = new IInterceptor[0];

        protected ImmutableLinkedList<ICondition> Conditions = ImmutableLinkedList<ICondition>.Empty;
        protected IInterceptor[] InsideLifestyleInterceptors;
        protected IInterceptor[] OutsideLifestyleInterceptors;
        protected Container Container;
        protected EditableMetadata EditableMetadata = new EditableMetadata();

        protected BasicStrategy(Container container) => Container = container;

        /// <summary>
        /// Activation type for strategy
        /// </summary>
        public abstract Type ActivationType { get; }

        /// <summary>
        /// Metadata for strategy
        /// </summary>
        public IMetadata Metadata => EditableMetadata;

        /// <summary>
        /// Lifestyle for strategy
        /// </summary>
        public virtual ILifestyle Lifestyle { get; set; }

        /// <summary>
        /// Is the strategy externally owned
        /// </summary>
        public virtual bool ExternallyOwned { get; set; }

        /// <summary>
        /// Does this strategy meet conditions to be used
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool MeetsCondition(InjectionContext context) =>
            Conditions.All(c => c.MeetsCondition(this, context));

        /// <summary>
        /// Activate strategy
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual object Activate(InjectionContext context)
        {
            if (InsideLifestyleInterceptors == null)
            {
                SetupInterceptors();
            }

            context.Strategy = this;

            if (OutsideLifestyleInterceptors != EmptyInterceptors)
            {
                foreach (var interceptor in OutsideLifestyleInterceptors)
                {
                    if (context.Instance == null)
                    {
                        interceptor.BeforeConstruction(context);
                    }
                }
            }

            if (Lifestyle != null)
            {
                Lifestyle.GetValue(context, Container, false, ActivateStrategy);
            }
            else
            {
                ActivateStrategy(context);

                if (!ExternallyOwned && context.Instance is IDisposable)
                {
                    context.Container.AddDisposable(context.Instance);
                }
            }

            if (OutsideLifestyleInterceptors != EmptyInterceptors)
            {
                foreach (var interceptor in OutsideLifestyleInterceptors)
                {
                    if (context.Instance == null)
                    {
                        interceptor.AfterConstruction(context);
                    }
                }
            }

            return context.Instance;
        }

        public virtual void AddCondition(ICondition condition)
        {
            Conditions = Conditions.Add(condition);
        }

        public virtual void AddMetadata(object key, object value)
        {
            EditableMetadata.AddData(key, value);
        }

        protected abstract void ActivateStrategy(InjectionContext context);

        protected virtual void SetupInterceptors()
        {
            var currentContainer = Container;
            var inside = ImmutableLinkedList<IInterceptor>.Empty;
            var outside = ImmutableLinkedList<IInterceptor>.Empty;

            while (currentContainer != null)
            {
                foreach (var interceptorProviders in currentContainer.Interceptors)
                {
                    var interceptor = interceptorProviders.ProvideInterceptor(this);

                    if (interceptor != null)
                    {
                        if (interceptor.InsideLifestyle)
                        {
                            inside = inside.Add(interceptor);
                        }
                        else
                        {
                            outside = outside.Add(interceptor);
                        }

                    }
                }

                currentContainer = currentContainer.Parent;
            }

            InsideLifestyleInterceptors = inside != ImmutableLinkedList<IInterceptor>.Empty ?
                inside.OrderByDescending(i => i.Order).ToArray() : EmptyInterceptors;

            OutsideLifestyleInterceptors = outside != ImmutableLinkedList<IInterceptor>.Empty ?
                outside.OrderByDescending(i => i.Order).ToArray() : EmptyInterceptors;
        }
    }
}
