using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AtomIoc.Data;
using AtomIoc.Strategies;
using AtomIoc.Tests.Classes;
using Xunit;

namespace AtomIoc.Tests.ContainerTests
{
    public class BasicContainerTests
    {
        [Fact]
        public void SimpleContainerTest()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var container = new Container();
            
            container.RegisterType<IBasicService, BasicService>();

            //var count = typeof(int).Assembly.ExportedTypes.Count();

            foreach (var type in typeof(int).Assembly.ExportedTypes)
            {
                container.RegisterType(type, type);
                //container.AddStrategy(new[] { type }, new KeyedType[0], new ReflectionStrategy(container, type));
            }

            var instance = container.Resolve<IBasicService>();
            stopWatch.Stop();
        }
    }
}
