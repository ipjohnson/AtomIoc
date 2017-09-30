using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Tests.Classes;
using Xunit;

namespace AtomIoc.Tests.ContainerTests
{
    public class KeyedContainerTests
    {
        [Fact]
        public void KeyedRegistration()
        {
            var container = new Container();

            container.RegisterType<IMultipleService, MultipleServiceA>("A");
            container.RegisterType<IMultipleService, MultipleServiceE>("E");

            var a = container.Resolve<IMultipleService>("A");

            Assert.NotNull(a);
            Assert.IsType<MultipleServiceA>(a);

            var e = container.Resolve<IMultipleService>("E");

            Assert.NotNull(e);
            Assert.IsType<MultipleServiceE>(e);
        }
    }
}
