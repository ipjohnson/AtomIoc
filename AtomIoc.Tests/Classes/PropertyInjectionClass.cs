using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Register;

namespace AtomIoc.Tests.Classes
{
    public class PropertyInjectionClass
    {
        [Dependency]
        public IBasicService BasicService { get; set; }
    }
}
