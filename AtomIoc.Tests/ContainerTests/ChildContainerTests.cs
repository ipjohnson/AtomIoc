using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Tests.Classes;
using Xunit;

namespace AtomIoc.Tests.ContainerTests
{
    public class ChildContainerTests
    {
        [Fact]
        public void ChildContainer_LocateDependencyInChild()
        {
            var container = new Container {_ => _.RegisterType<IDependsOnBasicService,DependsOnBasicService>()};
            
            using (var child = container.Child(_ => _.RegisterType<IBasicService,BasicService>()))
            {
                var service = child.Resolve<IDependsOnBasicService>();

                Assert.NotNull(service);
            }
        }
    }
}
