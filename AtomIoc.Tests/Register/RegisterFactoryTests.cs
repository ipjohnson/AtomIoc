using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Tests.Classes;
using Xunit;

namespace AtomIoc.Tests.Register
{
    public class RegisterFactoryTests
    {
        [Fact]
        public void RegisterFactory()
        {
            var container = new Container
            {
                _ =>
                _.RegisterType<IDependsOnBasicService, DependsOnBasicService>().
                  RegisterFactory<IBasicService>(context => new BasicService())
            };

            var instance = container.Resolve<IDependsOnBasicService>();

            Assert.NotNull(instance);
            Assert.NotNull(instance.BasicService);
        }
    }
}
