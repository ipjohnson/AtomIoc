using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using AtomIoc.Register;
using AtomIoc.Tests.Classes;
using Xunit;

namespace AtomIoc.Tests.ContainerTests
{
    public class MetaTests
    {
        [Fact]
        public void Meta()
        {
            var container = new Container();

            container.RegisterType<IBasicService, BasicService>(With.Metadata("Test", "Value"));

            var meta = container.Resolve<Meta<IBasicService>>();

            Assert.NotNull(meta);

            Assert.NotNull(meta.Value);

            Assert.Equal("Value", meta.Metadata.GetValueOrDefault<string>("Test"));
        }
    }
}
