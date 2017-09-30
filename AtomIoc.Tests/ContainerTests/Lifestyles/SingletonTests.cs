using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Tests.Classes;
using Xunit;

namespace AtomIoc.Tests.ContainerTests.Lifestyles
{
    public class SingletonTests
    {
        [Fact]
        public void Singleton()
        {
            var container = new Container();

            container.RegisterSingleton<IBasicService, BasicService>();

            var insance = container.Resolve<IBasicService>();

            Assert.NotNull(insance);

            var instance2 = container.Resolve<IBasicService>();

            Assert.NotNull(instance2);
        }
    }
}
