using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Register;
using AtomIoc.Tests.Classes;
using Xunit;

namespace AtomIoc.Tests.ContainerTests.Lifestyles
{
    public class NamedScopeLifestyleTests
    {
        [Fact]
        public void NamedScopeLifestyle()
        {
            var container = new Container();

            container.RegisterType<IBasicService, BasicService>(Lifestyle.NamedScope("Test"));

            using (var child = container.Child(name: "Test"))
            {
                var instance = child.Resolve<IBasicService>();

                Assert.NotNull(instance);

                using (var secondChild = child.Child())
                {
                    var secondInstance = secondChild.Resolve<IBasicService>();

                    Assert.NotNull(secondInstance);
                    Assert.Same(instance, secondInstance);
                }
            }
        }

        [Fact]
        public void NamedScopeLifestyle_ThrowsException()
        {
            var container = new Container();

            container.RegisterType<IBasicService, BasicService>(Lifestyle.NamedScope("Test"));

            Assert.Throws<Exception>(() => container.Resolve<IBasicService>());
        }
    }
}
