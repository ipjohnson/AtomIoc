using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AtomIoc.Data;
using AtomIoc.Interfaces;
using AtomIoc.Lifestyles;
using AtomIoc.Strategies;

namespace AtomIoc
{
    public class Container : DisposalScope, IScope, IEnumerable<IStrategy>
    {
        protected static ImmutableHashTree<Type, StrategyCollection> DefaultStrategies =
            ImmutableHashTree<Type, StrategyCollection>.Empty;

        static Container()
        {
            var strategy = new ContainerStrategy();

            ImmutableHashTree.ThreadSafeAdd(ref DefaultStrategies, typeof(Container), new StrategyCollection()).AddStrategy(strategy);
            ImmutableHashTree.ThreadSafeAdd(ref DefaultStrategies, typeof(IScope), new StrategyCollection()).AddStrategy(strategy);
            ImmutableHashTree.ThreadSafeAdd(ref DefaultStrategies, typeof(IEnumerable<>), new StrategyCollection()).AddStrategy(new EnumerableStrategy());
        }

        protected ImmutableLinkedList<IInterceptorProvider> InterceptorsField = ImmutableLinkedList<IInterceptorProvider>.Empty;
        protected ImmutableLinkedList<IMemberInjectionSelector> MemberInjectionSelectorsField = ImmutableLinkedList<IMemberInjectionSelector>.Empty;
        protected ImmutableHashTree<object, object> ExtraData = ImmutableHashTree<object, object>.Empty;
        protected ImmutableHashTree<Type, StrategyCollection> Strategies = DefaultStrategies;
        protected ImmutableHashTree<KeyedType, StrategyCollection> KeyedStrategies = ImmutableHashTree<KeyedType, StrategyCollection>.Empty;

        public Container(ContainerConfiguration configuration = null, string name = null)
        {
            Configuration = configuration ?? new ContainerConfiguration();
            Name = name ?? "";
        }

        public ContainerConfiguration Configuration { get; }

        public Container Parent { get; protected set; }

        public string Name { get; }

        public IEnumerable<IStrategy> FindAllStrategies(InjectionContext injectionContext,
            Func<IStrategy, InjectionContext, bool> filter = null)
        {
            StrategyCollection strategies = injectionContext.Key == null ?
                Strategies.GetValueOrDefault(injectionContext.RequesType) :
                KeyedStrategies.GetValueOrDefault(new KeyedType(injectionContext.RequesType, injectionContext.Key));

            if (strategies != null)
            {
                var filteredStrategies =
                    strategies.Strategies.Where(s => s.MeetsCondition(injectionContext) &&
                                                     (filter?.Invoke(s, injectionContext) ?? true)).ToArray();

                // reverising order
                for (var i = filteredStrategies.Length - 1; i >= 0; i--)
                {
                    yield return filteredStrategies[i];
                }
            }

            if (Parent != null)
            {
                foreach (var strategy in Parent.FindAllStrategies(injectionContext, filter))
                {
                    yield return strategy;
                }
            }
        }

        public IStrategy FindStrategy(InjectionContext injectionContext, Func<IStrategy, InjectionContext, bool> filter = null)
        {
            StrategyCollection strategies = injectionContext.Key == null ?
                Strategies.GetValueOrDefault(injectionContext.RequesType) :
                KeyedStrategies.GetValueOrDefault(new KeyedType(injectionContext.RequesType, injectionContext.Key));

            if (strategies == null && injectionContext.RequesType.IsConstructedGenericType)
            {
                var openGenericType = injectionContext.RequesType.GetGenericTypeDefinition();

                strategies = injectionContext.Key == null ?
                    Strategies.GetValueOrDefault(openGenericType) :
                    KeyedStrategies.GetValueOrDefault(new KeyedType(openGenericType, injectionContext.Key));
            }

            if (strategies != null)
            {
                var filteredStrategies =
                    strategies.Strategies.Where(s => s.MeetsCondition(injectionContext) &&
                                                     (filter?.Invoke(s, injectionContext) ?? true)).ToArray();

                if (filteredStrategies.Length == 1)
                {
                    return filteredStrategies[0];
                }

                foreach (var picker in injectionContext.StrategyPicker)
                {
                    var strategy = picker(filteredStrategies, injectionContext);

                    if (strategy != null)
                    {
                        return strategy;
                    }
                }
            }
            else if (injectionContext.RequesType.IsArray)
            {
                return new ArrayStrategy();
            }
            
            return Parent?.FindStrategy(injectionContext, filter);
        }

        /// <summary>
        /// List of interc
        /// </summary>
        public IEnumerable<IInterceptorProvider> Interceptors => InterceptorsField;

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IMemberInjectionSelector> MemberInjectionSelectors => MemberInjectionSelectorsField;

        /// <summary>
        /// Enumeration of all the key value pairs
        /// </summary>
        public IEnumerable<KeyValuePair<object, object>> KeyValuePairs => ExtraData;

        /// <summary>
        /// Extra data associated with the injection request. 
        /// </summary>
        /// <param name="key">key of the data object to get</param>
        /// <returns>data value</returns>
        public object GetExtraData(object key) => ExtraData.GetValueOrDefault(key);

        /// <summary>
        /// Sets extra data on the injection context
        /// </summary>
        /// <param name="key">object name</param>
        /// <param name="newValue">new object value</param>
        /// <param name="replaceIfExists">replace value if key exists</param>
        /// <returns>the final value of key</returns>
        public object SetExtraData(object key, object newValue, bool replaceIfExists = true) =>
            ImmutableHashTree.ThreadSafeAdd(ref ExtraData, key, newValue, replaceIfExists);

        /// <summary>
        /// Implementing as interface to hide it 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="withKey"></param>
        /// <param name="extraData"></param>
        /// <param name="required"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        object IScope.Locate(Type type, object withKey, object extraData, bool required, Func<IStrategy, InjectionContext, bool> filter)
        {
            var context = new InjectionContext(type, withKey, this, extraData);

            var strategy = FindStrategy(context, filter);

            context.Strategy = strategy;

            return strategy != null ?
                strategy.Activate(context) :
                !required ? (object)null : throw new Exception($"Could not locate type {type.Name}");
        }

        public virtual Container Child(Action<Container> configure = null, string name = null)
        {
            var newContainer = new Container(Configuration, name) { Parent = this };

            configure?.Invoke(newContainer);

            return newContainer;
        }

        public Container AddStrategy(IEnumerable<Type> asTypes, IEnumerable<KeyedType> asKeyedTypes, IStrategy strategy)
        {
            foreach (var type in asTypes)
            {
                var collection = Strategies.GetValueOrDefault(type) ??
                    ImmutableHashTree.ThreadSafeAdd(ref Strategies, type, new StrategyCollection());

                collection.AddStrategy(strategy);
            }

            foreach (var asKeyedType in asKeyedTypes)
            {
                var keyedType = new KeyedType(asKeyedType.Type, asKeyedType.Key);

                var collection = KeyedStrategies.GetValueOrDefault(keyedType) ??
                                 ImmutableHashTree.ThreadSafeAdd(ref KeyedStrategies, keyedType, new StrategyCollection());

                collection.AddStrategy(strategy);
            }

            return this;
        }

        /// <summary>
        /// Add container action
        /// </summary>
        /// <param name="addAction"></param>
        /// <returns></returns>
        public Container Add(Action<Container> addAction)
        {
            addAction(this);

            return this;
        }

        /// <summary>
        /// Add interceptor provider
        /// </summary>
        /// <param name="interceptorProvider"></param>
        /// <returns></returns>
        public Container AddInspector(IInterceptorProvider interceptorProvider)
        {
            InterceptorsField = InterceptorsField.Add(interceptorProvider);

            return this;
        }

        /// <summary>
        /// Add Member injction selector
        /// </summary>
        /// <param name="memberInjectionSelector"></param>
        /// <returns></returns>
        public Container AddMemberInjctionSelector(IMemberInjectionSelector memberInjectionSelector)
        {
            MemberInjectionSelectorsField = MemberInjectionSelectorsField.Add(memberInjectionSelector);

            return this;
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<IStrategy> IEnumerable<IStrategy>.GetEnumerator()
        {
            var returnList = new List<IStrategy>();

            return returnList.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<IStrategy>)this).GetEnumerator();
        }
    }
}
