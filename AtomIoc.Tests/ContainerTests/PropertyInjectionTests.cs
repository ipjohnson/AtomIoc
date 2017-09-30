using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using AtomIoc.Register;
using AtomIoc.Tests.Classes;
using Xunit;

namespace AtomIoc.Tests.ContainerTests
{
    public class PropertyInjectionTests
    {
        [Fact]
        public void Property_DependencyInjection()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var container = new Container();

            container.InjectAttributed();

            container.RegisterType<IBasicService, BasicService>();
            container.RegisterType<PropertyInjectionClass, PropertyInjectionClass>();

            var instance = container.Resolve<PropertyInjectionClass>();

            Assert.NotNull(instance);
            Assert.NotNull(instance.BasicService);
            stopWatch.Stop();
        }
    }
}
