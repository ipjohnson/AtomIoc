using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AtomIoc.Conditions;
using AtomIoc.Interfaces;

namespace AtomIoc.Register
{
    public class Conditions
    {
        public ICondition IsTrue(Func<IStrategy, InjectionContext, bool> match)
        {
            return new MatchCondition(match);
        }

        public ICondition IsTrue<T>(Func<T, bool> condition)
        {
            return new MatchCondition((strategy, context) => condition(context.Resolve<T>()));
        }

        public ICondition TargetHas<T>() where T : Attribute
        {
            return new MatchCondition((strategy, context) =>
            {
                if (context.MemberInjection is ParameterInfo parameterInfo)
                {
                    if (parameterInfo.GetCustomAttribute<T>() != null)
                    {
                        return true;
                    }

                    return parameterInfo.Member.DeclaringType.GetTypeInfo().GetCustomAttribute<T>() != null;
                }

                if (context.MemberInjection is PropertyInfo propertyInfo)
                {
                    if (propertyInfo.GetCustomAttribute<T>() != null)
                    {
                        return true;
                    }

                    return propertyInfo.DeclaringType.GetTypeInfo().GetCustomAttribute<T>() != null;
                }

                if (context.MemberInjection is FieldInfo fieldInfo)
                {
                    if (fieldInfo.GetCustomAttribute<T>() != null)
                    {
                        return true;
                    }

                    return fieldInfo.DeclaringType.GetTypeInfo().GetCustomAttribute<T>() != null;
                }

                return false;
            });
        }
        

    }
}
