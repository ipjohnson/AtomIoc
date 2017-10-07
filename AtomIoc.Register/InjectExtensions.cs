using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using AtomIoc.Interfaces;

namespace AtomIoc.Register
{
    public class DependencyAttributeMemberSelector<T> : IMemberInjectionSelector where T : Attribute
    {
        public class PropertyInjector : IMemberInjectionInfo
        {
            private PropertyInfo _member;
            private object _key;

            public PropertyInjector(PropertyInfo member, object key)
            {
                _member = member;
                _key = key;
            }

            public MemberInfo Member => _member;

            public Action<InjectionContext> MemberInjectionAction()
            {
                var member = _member;
                var key = _key;

                return context =>
                {
                    member.SetValue(context.Instance, context.Resolve(member.PropertyType, key));
                };
            }       
        }

        public IEnumerable<IMemberInjectionInfo> GetMembersToInject(IEnumerable<MemberInfo> members)
        {
            foreach (var member in members)
            {
                if (member is PropertyInfo propertyInfo)
                {
                    var attr = member.GetCustomAttribute<DependencyAttribute>();

                    if (attr != null)
                    {
                        yield return new PropertyInjector(propertyInfo, attr.Key);
                    }
                }
            }
        }
    }

    public static class InjectExtensions
    {
        public static Container InjectAttributed(this Container container)
        {
            container.AddMemberInjctionSelector(new DependencyAttributeMemberSelector<DependencyAttribute>());

            return container;
        }
        
    }
}
