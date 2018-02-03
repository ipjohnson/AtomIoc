using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Tests.Classes;
using Xunit;

namespace AtomIoc.Tests.ContainerTests.FuncTests
{
    public class NoArgFuncTests
    {
        [Fact]
        public void NoArgFunc()
        {
            var container = new Container();

            container.RegisterType<IBasicService, BasicService>();

            var func = container.Resolve<Func<IBasicService>>();

            Assert.NotNull(func);

            var instance = func();

            Assert.NotNull(instance);
            Assert.IsType<BasicService>(instance);
        }
    }
}
