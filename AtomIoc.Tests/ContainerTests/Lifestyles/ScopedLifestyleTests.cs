using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Register;
using AtomIoc.Tests.Classes;
using Xunit;

namespace AtomIoc.Tests.ContainerTests.Lifestyles
{
    public class ScopedLifestyleTests
    {
        [Fact]
        public void ScopedLifestyle()
        {
            var container = new Container();

            container.RegisterType<IBasicService, BasicService>(Lifestyle.Scoped());

            var instance = container.Resolve<IBasicService>();

            Assert.NotNull(instance);

            var instance2 = container.Resolve<IBasicService>();

            Assert.NotNull(instance2);

            Assert.Same(instance, instance2);

            using (var child = container.Child())
            {
                var childInstance = child.Resolve<IBasicService>();

                Assert.NotNull(childInstance);

                var childInstance2 = child.Resolve<IBasicService>();

                Assert.NotNull(childInstance2);

                Assert.Same(childInstance, childInstance2);
                Assert.NotSame(instance, childInstance);
            }
        }
    }
}
