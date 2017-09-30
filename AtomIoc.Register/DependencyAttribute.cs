using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Register
{
    public class DependencyAttribute : Attribute
    {
        public object Key { get; set; }
    }
}
