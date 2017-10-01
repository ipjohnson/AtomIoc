using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Tests.Classes;
using Xunit;

namespace AtomIoc.Tests.ContainerTests
{
    public class DisposalScopeTests
    {
        [Fact]
        public void DisposalScope_DisposesIsntance()
        {
            var container = new Container();

            container.RegisterType<IDisposableService, DisposableService>();
            
            var instance = container.Resolve<IDisposableService>();

            Assert.NotNull(instance);

            bool instanceDisposed = false;

            instance.Disposed += (sender, args) => instanceDisposed = true;

            bool childInstanceDisposed = false;

            using (var childContainer = container.Child())
            {
                var childInstance = childContainer.Resolve<IDisposableService>();

                childInstance.Disposed += (sender, args) => childInstanceDisposed = true;
            }

            Assert.True(childInstanceDisposed);
            Assert.False(instanceDisposed);

            container.Dispose();

            Assert.True(instanceDisposed);
        }
    }
}
