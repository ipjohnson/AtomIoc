using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AtomIoc.Utilities
{
    public class TypesThatFilter : GenericFilterGroup<Type>
    {
        /// <summary>
        /// Tests to see if a type has an attribute
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="attributeFilter"></param>
        /// <returns></returns>
        public TypesThatFilter HaveAttribute<TAttribute>(Func<TAttribute, bool> attributeFilter = null)
            where TAttribute : Attribute
        {
            Func<Type, bool> newFilter;

            if (attributeFilter != null)
            {
                newFilter = t => t.GetTypeInfo().GetCustomAttributes(true).
                    Any(
                    x =>
                    {
                        if (x is TAttribute attribute)
                        {
                            return  attributeFilter(attribute);
                        }

                        return false;
                    });
            }
            else
            {
                newFilter = t => t.GetTypeInfo().GetCustomAttributes(typeof(TAttribute), true).Any();
            }

            Add(newFilter);

            return this;
        }

    }
}
