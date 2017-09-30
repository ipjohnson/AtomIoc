using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AtomIoc.Interfaces;

namespace AtomIoc.Strategies
{
    public class ArrayStrategy : IStrategy
    {
        /// <summary>
        /// Activation type for strategy
        /// </summary>
        public Type ActivationType => typeof(Array);

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
            var elementtype = context.RequesType.GetElementType();

            var strategies = context.Container.FindAllStrategies(context.Child(elementtype)).ToArray();

            var array = Array.CreateInstance(elementtype, strategies.Length);

            for (int i = 0; i < strategies.Length; i++)
            {
                array.SetValue(strategies[i].Activate(context.Child(elementtype)), i);
            }

            return array;
        }

        /// <summary>
        /// Does this strategy meet conditions to be used
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool MeetsCondition(InjectionContext context) => true;
    }
}
