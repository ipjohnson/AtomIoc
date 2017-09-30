using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Interfaces;

namespace AtomIoc.Utilities
{
    public class WithConfiguration
    {
        public static implicit operator IDependencyProvider[](WithConfiguration configuration)
        {
            return null;
        }
    }
}
