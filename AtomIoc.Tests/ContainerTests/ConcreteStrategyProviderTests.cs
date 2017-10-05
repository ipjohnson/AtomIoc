using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Strategies;
using AtomIoc.Tests.Classes;
using Xunit;

namespace AtomIoc.Tests.ContainerTests
{
    public class ConcreteStrategyProviderTests
    {
        [Fact]
        public void ConcreteStrategyProvider_CreateType()
        {
            var container = new Container();

            var instance = container.Resolve<BasicService>();

            Assert.NotNull(instance);
        }
    }
}
