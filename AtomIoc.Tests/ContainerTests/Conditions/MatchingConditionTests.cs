using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Register;
using AtomIoc.Tests.Classes;
using Xunit;

namespace AtomIoc.Tests.ContainerTests.Conditions
{
    public class MatchingConditionTests
    {
        [Some]
        public class AttributeDependency : IDependsOnBasicService
        {
            public AttributeDependency(IBasicService basicService)
            {
                BasicService = basicService;
            }

            public IBasicService BasicService { get; }
        }

        [Fact]
        public void WithConditionHasAttribute()
        {
            var container = new Container();

            container.RegisterType<IBasicService, BasicService>();
            container.RegisterType<IBasicService, OtherBasicService>(With.Condition.TargetHas<SomeAttribute>());
            container.RegisterType<AttributeDependency, AttributeDependency>();
            container.RegisterType<DependsOnBasicService, DependsOnBasicService>();

            var dependency = container.Resolve<AttributeDependency>();

            Assert.NotNull(dependency);
            Assert.NotNull(dependency.BasicService);
            Assert.IsType<OtherBasicService>(dependency.BasicService);

            var service = container.Resolve<DependsOnBasicService>();

            Assert.NotNull(service);
            Assert.NotNull(service.BasicService);
            Assert.IsType<BasicService>(service.BasicService);
        }

        public class SomeClass
        {
            public bool Value { get; set; }
        }


        [Fact]
        public void WithConditionIsTrue()
        {
            var container = new Container();

            var someClass = new SomeClass { Value = true };

            container.RegisterFactory(context => someClass);
            container.RegisterType<IBasicService, BasicService>();
            container.RegisterType<IBasicService, OtherBasicService>(With.Condition.IsTrue((strategy, context) => context.Resolve<SomeClass>().Value));
            container.RegisterType<IDependsOnBasicService, DependsOnBasicService>();

            var instance = container.Resolve<IDependsOnBasicService>();

            Assert.NotNull(instance);
            Assert.NotNull(instance.BasicService);
            Assert.IsType<OtherBasicService>(instance.BasicService);

            someClass.Value = false;

            var instance2 = container.Resolve<IDependsOnBasicService>();

            Assert.NotNull(instance2);
            Assert.NotNull(instance2.BasicService);
            Assert.IsType<BasicService>(instance2.BasicService);
        }

        [Fact]
        public void WithConditionIsTrueGeneric()
        {
            var container = new Container();

            var someClass = new SomeClass { Value = true };

            container.RegisterFactory(context => someClass);
            container.RegisterType<IBasicService, BasicService>();
            container.RegisterType<IBasicService, OtherBasicService>(With.Condition.IsTrue<SomeClass>(_ => _.Value));
            container.RegisterType<IDependsOnBasicService, DependsOnBasicService>();

            var instance = container.Resolve<IDependsOnBasicService>();

            Assert.NotNull(instance);
            Assert.NotNull(instance.BasicService);
            Assert.IsType<OtherBasicService>(instance.BasicService);

            someClass.Value = false;

            var instance2 = container.Resolve<IDependsOnBasicService>();

            Assert.NotNull(instance2);
            Assert.NotNull(instance2.BasicService);
            Assert.IsType<BasicService>(instance2.BasicService);
        }
    }
}
