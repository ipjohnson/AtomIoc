﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AtomIoc.Tests.Classes;
using Xunit;

namespace AtomIoc.Tests.ContainerTests.Enumerables
{
    public class ArrayTests
    {
        [Fact]
        public void Array()
        {
            var container = new Container();

            container.RegisterType<IMultipleService, MultipleServiceA>();
            container.RegisterType<IMultipleService, MultipleServiceB>();
            container.RegisterType<IMultipleService, MultipleServiceC>();
            container.RegisterType<IMultipleService, MultipleServiceD>();
            container.RegisterType<IMultipleService, MultipleServiceE>();

            var multipleServices = container.Resolve<IMultipleService[]>();

            Assert.NotNull(multipleServices);
            Assert.Equal(5, multipleServices.Length);

        }
    }
}
