using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Data;
using AtomIoc.Interfaces;
using AtomIoc.Lifestyles;
using AtomIoc.Register;
using AtomIoc.Strategies;

namespace AtomIoc
{
    public static class RegisterExtensions
    {
        /// <summary>
        /// Is type registered
        /// </summary>
        /// <param name="container"></param>
        /// <param name="type"></param>
        /// <param name="withKey"></param>
        /// <returns></returns>
        public static bool IsRegistered(this Container container, Type type, object withKey = null)
        {
            return container.FindStrategy(new InjectionContext(type, withKey, container, null)) != null;
        }

        /// <summary>
        /// Is type registered
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <param name="withKey"></param>
        /// <returns></returns>
        public static bool IsRegistered<T>(this Container container, object withKey = null)
        {
            return container.FindStrategy(new InjectionContext(typeof(T), withKey, container, null)) != null;
        }
        public static Container RegisterType(this Container container, Type registerType, Type implementation, params object[] configurationObjects)
        {
            var strategy =
                new ReflectionStrategy(container, implementation);

            var withKey = (object)null;

            if (configurationObjects != null && configurationObjects.Length > 0)
            {
                ProcessConfigurationObjects(configurationObjects, ref withKey, strategy);
            }

            return AddStrategyToContainer(container, registerType, withKey, strategy);
        }

        /// <summary>
        /// Register type
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="container"></param>
        /// <param name="configurationObjects"></param>
        /// <returns></returns>
        public static Container RegisterType<TInterface, TImplementation>(this Container container, params object[] configurationObjects) where TImplementation : TInterface
        {
            var strategy =
                new ReflectionStrategy(container, typeof(TImplementation));

            object withKey = null;

            if (configurationObjects != null && configurationObjects.Length > 0)
            {
                ProcessConfigurationObjects(configurationObjects, ref withKey, strategy);
            }

            return AddStrategyToContainer(container, typeof(TInterface), withKey, strategy);
        }

        /// <summary>
        /// Register Singleton
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="container"></param>
        /// <param name="configurationObjects"></param>
        /// <returns></returns>
        public static Container RegisterSingleton<TInterface, TImplementation>(this Container container, params object[] configurationObjects) where TImplementation : TInterface
        {
            var strategy =
                new ReflectionStrategy(container, typeof(TImplementation)) { Lifestyle = new SingletonLifestyle() };

            object withKey = null;

            if (configurationObjects != null && configurationObjects.Length > 0)
            {
                ProcessConfigurationObjects(configurationObjects, ref withKey, strategy);
            }

            return AddStrategyToContainer(container, typeof(TInterface), withKey, strategy);
        }

        /// <summary>
        /// Register Factory
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="container"></param>
        /// <param name="factory"></param>
        /// <param name="configurationObjects"></param>
        /// <returns></returns>
        public static Container RegisterFactory<TResult>(this Container container,
            Func<InjectionContext, TResult> factory, params object[] configurationObjects)
        {
            var strategy = new ContextArgStrategy<TResult>(container, factory);

            object withKey = null;

            if (configurationObjects != null && configurationObjects.Length > 0)
            {
                ProcessConfigurationObjects(configurationObjects, ref withKey, strategy);
            }
            return AddStrategyToContainer(container, typeof(TResult), withKey, strategy);
        }

        private static Container AddStrategyToContainer(Container container, Type type, object withKey,
            IStrategy strategy)
        {
            if (withKey == null)
            {
                container.AddStrategy(new[] { type }, ImmutableLinkedList<KeyedType>.Empty, strategy);
            }
            else
            {
                container.AddStrategy(ImmutableLinkedList<Type>.Empty,
                    new[] { new KeyedType(type, withKey) }, strategy);
            }

            return container;
        }

        private static void ProcessConfigurationObjects(object[] configurationObjects, ref object withKey,
            IConfigurableStrategy strategy)
        {
            foreach (var o in configurationObjects)
            {
                switch (o)
                {
                    case ILifestyle lifestyle:
                        strategy.Lifestyle = lifestyle;
                        break;

                    case KeyValuePair<object, object> metadata:
                        strategy.AddMetadata(metadata.Key, metadata.Value);
                        break;

                    case ICondition condition:
                        strategy.AddCondition(condition);
                        break;

                    default:
                        withKey = o;
                        break;
                }
            }
        }

    }
}
