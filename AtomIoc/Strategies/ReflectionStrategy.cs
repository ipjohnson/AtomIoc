using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AtomIoc.Data;
using AtomIoc.Interfaces;

namespace AtomIoc.Strategies
{
    public class ReflectionStrategy : BasicStrategy, IConfigurableStrategy
    {
        protected ImmutableLinkedList<Action<InjectionContext>> MemberInjection;
        protected ConstructorInfo CachedConstructor;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="container"></param>
        /// <param name="activationType"></param>
        public ReflectionStrategy(Container container, Type activationType) :
            base(container) => ActivationType = activationType;

        /// <summary>
        /// Activation type
        /// </summary>
        public override Type ActivationType { get; }

        /// <summary>
        /// Activate the strategy
        /// </summary>
        /// <param name="context"></param>
        protected override void ActivateStrategy(InjectionContext context)
        {
            object instance = null;
            if (CachedConstructor != null)
            {
                instance = AttemptToInvokeConstructor(context, CachedConstructor);

            }
            else
            {
                var constructors =
                    ActivationType.GetTypeInfo().DeclaredConstructors.Where(c => c.IsPublic && !c.IsStatic)
                        .OrderByDescending(c => c.GetParameters().Length).ToArray();

                if (constructors.Length == 1)
                {
                    CachedConstructor = constructors[0];
                }

                foreach (var constructor in constructors)
                {
                    instance = AttemptToInvokeConstructor(context, constructor);

                    if (instance != null)
                    {
                        break;
                    }
                }
            }

            if (instance != null)
            {
                context.Instance = instance;

                if (MemberInjection == null)
                {
                    SetupMemberInjectionList();
                }

                MemberInjection.Visit(action => action(context));
            }
            else if (context.IsRequired)
            {
                throw new Exception($"Could not find constructor on type {ActivationType.FullName} to call");
            }
        }

        private void SetupMemberInjectionList()
        {
            var currentContainer = Container;
            ImmutableLinkedList<IMemberInjectionSelector> selectors = ImmutableLinkedList<IMemberInjectionSelector>.Empty;

            while (currentContainer != null)
            {
                selectors = selectors.AddRange(currentContainer.MemberInjectionSelectors);

                currentContainer = currentContainer.Parent;
            }

            if (selectors == ImmutableLinkedList<IMemberInjectionSelector>.Empty)
            {
                MemberInjection = ImmutableLinkedList<Action<InjectionContext>>.Empty;
                return;
            }
            
            var members = new List<MemberInfo>();

            foreach (var runtimeProperty in ActivationType.GetRuntimeProperties())
            {
                if (!runtimeProperty.CanWrite ||
                    !runtimeProperty.SetMethod.IsPublic || 
                     runtimeProperty.SetMethod.IsStatic)
                {
                    continue;
                }

                members.Add(runtimeProperty);
            }

            foreach (var method in ActivationType.GetRuntimeMethods())
            {
                if (method.IsStatic || !method.IsPublic)
                {
                    continue;
                }

                members.Add(method);
            }

            var injectMembers = new Dictionary<MemberInfo, IMemberInjectionInfo>(); 

            if (members.Count > 0)
            {
                selectors.Visit(m =>
                {
                    foreach (var info in m.GetMembersToInject(members))
                    {
                        injectMembers[info.Member] = info;
                    }
                });

                var injectionMembers = ImmutableLinkedList<Action<InjectionContext>>.Empty;

                foreach (var action in
                    injectMembers.Values.OrderBy(m => m.Member is PropertyInfo).Select(m => m.MemberInjectionAction()))
                {
                    injectionMembers = injectionMembers.Add(action);
                }

                MemberInjection = injectionMembers;
            }
        }

        public static object AttemptToInvokeConstructor(InjectionContext context, ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();

            var values = new object[parameters.Length];
            var i = 0;

            for (; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                if (context.ExtraData != null)
                {
                    var value = context.GetExtraData(parameter.Name.ToLower());

                    if (value != null &&
                        parameter.ParameterType.GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo()))
                    {
                        values[i] = value;
                        continue;
                    }

                    value = context.ExtraDataProperties.FirstOrDefault(o =>
                        parameter.ParameterType.GetTypeInfo().IsAssignableFrom(o.GetType().GetTypeInfo()));

                    if (value != null)
                    {
                        values[i] = value;
                        continue;
                    }
                }

                var strategy =
                    context.Container.FindStrategy(context.Child(parameter.ParameterType,
                        context.Container.Configuration.KeyFunction(parameter.ParameterType)
                            ? parameter.Name
                            : null,
                        parameter));

                if (strategy == null)
                {
                    break;
                }

                values[i] = strategy.Activate(context);
            }

            return i == parameters.Length ? constructor.Invoke(values) : null;
        }
    }
}
