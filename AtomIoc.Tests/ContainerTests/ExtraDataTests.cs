using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Tests.Classes;
using Xunit;

namespace AtomIoc.Tests.ContainerTests
{
    public class ExtraDataTests
    {
        [Fact]
        public void ExtraData()
        {
            var container = new Container { _ => _.RegisterType<IDependsOnBasicService, DependsOnBasicService>() };

            var basicService = new BasicService();

            var instance = container.Resolve<IDependsOnBasicService>(extraData: new { basicService });

            Assert.NotNull(instance);
            Assert.NotNull(instance.BasicService);
        }


        [Fact]
        public void ExtraData_DifferentName_TypeMatch()
        {
            var container = new Container { _ => _.RegisterType<IDependsOnBasicService, DependsOnBasicService>() };

            var basicService = new BasicService();

            var instance = container.Resolve<IDependsOnBasicService>(extraData: new { someOtherName = basicService });

            Assert.NotNull(instance);
            Assert.NotNull(instance.BasicService);
        }
    }
}
