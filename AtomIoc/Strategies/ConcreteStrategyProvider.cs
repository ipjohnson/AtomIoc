using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AtomIoc.Interfaces;

namespace AtomIoc.Strategies
{
    public class ConcreteStrategyProvider : IMissingStrategyProvider
    {
        public IEnumerable<StrategyInformation> ProvideStrategies(Container container, InjectionContext injectionContext)
        {
            var typeInfo = injectionContext.RequesType.GetTypeInfo();

            if (typeInfo.IsInterface || 
                typeInfo.IsAbstract)
            {
                yield break;
            }

            if (injectionContext.RequesType.IsConstructedGenericType)
            {
                var genericType = injectionContext.RequesType.GetGenericTypeDefinition();

                if (genericType == typeof(Meta<>))
                {
                    yield return new StrategyInformation{ As = new []{typeof(Meta<>)}, Strategy = new MetaStrategy()};
                    yield break;
                }
            }

            if (typeInfo.DeclaredConstructors.Any(c => c.IsPublic && !c.IsAbstract))
            {
                yield return new StrategyInformation
                {
                    As = new[] { injectionContext.RequesType },
                    Strategy = new ReflectionStrategy(container, injectionContext.RequesType)
                };
            }
        }
    }
}
