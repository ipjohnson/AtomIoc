using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Interfaces;
using AtomIoc.Register;
using Xunit;

namespace AtomIoc.Tests.ContainerTests
{
    public class InitializeTests
    {
        public interface IInitializeTestService
        {
            int Value { get; }

            void Init();
        }

        public class InitTestService : IInitializeTestService
        {
            public int Value { get; private set; }

            public void Init()
            {
                Value = 5;
            }
        }

        [Fact]
        public void InitializeType()
        {
            var container = new Container();

            container.Initialize<IInitializeTestService>(service => service.Init());
            container.RegisterType<IInitializeTestService, InitTestService>();

            var instance = container.Resolve<IInitializeTestService>();

            Assert.NotNull(instance);
            Assert.Equal(5, instance.Value);
        }
    }
}
