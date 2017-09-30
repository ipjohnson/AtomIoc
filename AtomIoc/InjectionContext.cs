using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AtomIoc.Data;
using AtomIoc.Interfaces;

namespace AtomIoc
{
    public class InjectionContext : IExtraDataContainer
    {
        public static readonly ImmutableLinkedList<Func<IEnumerable<IStrategy>, InjectionContext, IStrategy>> DefaultPicker =
            ImmutableLinkedList<Func<IEnumerable<IStrategy>, InjectionContext, IStrategy>>.Empty.Add(DefaultMethod);

        private static IStrategy DefaultMethod(IEnumerable<IStrategy> strategies, InjectionContext context)
        {
            return strategies.FirstOrDefault();
        }

        protected ImmutableLinkedList<object> ExtraDataPropertiesList = ImmutableLinkedList<object>.Empty;
        protected ImmutableHashTree<object, object> FrameworkData = ImmutableHashTree<object, object>.Empty;
        protected ImmutableHashTree<object, object> ExtraDataTree = ImmutableHashTree<object, object>.Empty;

        public InjectionContext(Type requesType, object key, Container container, object extraData, object memberInjection = null)
        {
            IsRequired = true;
            StrategyPicker = DefaultPicker;
            RequesType = requesType;
            Key = key;
            Container = container;
            ExtraData = extraData;
            MemberInjection = memberInjection;
            ProcessExtraData(extraData);
        }

        protected void ProcessExtraData(object extraData)
        {
            if (extraData == null)
            {
                return;
            }

            if (extraData is IReadOnlyDictionary<object, object> extraDataDictionary)
            {
                foreach (var data in extraDataDictionary)
                {
                    ExtraDataTree = ExtraDataTree.Add(data.Key, data.Value);
                    ExtraDataPropertiesList = ExtraDataPropertiesList.Add(data.Value);
                }
            }
            else if (extraData is Array extraDataArray)
            {
                for (int i = extraDataArray.Length - 1; i >= 0; i--)
                {
                    ExtraDataPropertiesList = ExtraDataPropertiesList.Add(extraDataArray.GetValue(i));
                }
            }
            else
            {
                foreach (var property in extraData.GetType().GetRuntimeProperties())
                {
                    if (property.CanRead && !property.GetMethod.IsStatic)
                    {
                        var value = property.GetValue(extraData);

                        ExtraDataTree = ExtraDataTree.Add(property.Name.ToLower(), value);
                        ExtraDataPropertiesList = ExtraDataPropertiesList.Add(value);
                    }
                }
            }
        }

        public Type RequesType { get; }

        public object Key { get; }

        public Container Container { get; }

        public object ExtraData { get; protected set; }

        public IStrategy Strategy { get; set; }

        public object MemberInjection { get; }

        public virtual InjectionContext Parent { get; protected set; }

        public object Instance { get; set; }

        public bool IsRequired { get; set; }

        public ImmutableLinkedList<Func<IEnumerable<IStrategy>, InjectionContext, IStrategy>> StrategyPicker { get; set; }

        public IEnumerable<object> ExtraDataProperties => ExtraDataPropertiesList;

        public virtual InjectionContext Child(Type requestType, object key = null, object memberInjection = null)
        {
            return new InjectionContext(requestType, key, Container, null, memberInjection)
            {
                Parent = this,
                ExtraDataTree = ExtraDataTree,
                ExtraData = ExtraData,
                StrategyPicker = StrategyPicker
            };
        }

        /// <summary>
        /// Enumeration of all the key value pairs
        /// </summary>
        public IEnumerable<KeyValuePair<object, object>> KeyValuePairs => ExtraDataTree;

        /// <summary>
        /// Extra data associated with the injection request. 
        /// </summary>
        /// <param name="key">key of the data object to get</param>
        /// <returns>data value</returns>
        public object GetExtraData(object key)
        {
            return ExtraDataTree.GetValueOrDefault(key);
        }

        /// <summary>
        /// Sets extra data on the injection context
        /// </summary>
        /// <param name="key">object name</param>
        /// <param name="newValue">new object value</param>
        /// <param name="replaceIfExists">replace value if key exists</param>
        /// <returns>the final value of key</returns>
        public object SetExtraData(object key, object newValue, bool replaceIfExists = true)
        {
            return ImmutableHashTree.ThreadSafeAdd(ref ExtraDataTree, key, newValue, replaceIfExists);
        }

        public object FrameworkDataGet(string key)
        {
            return null;
        }

        public object FrameworkDataSet(string key, object value)
        {
            return null;
        }
    }
}
