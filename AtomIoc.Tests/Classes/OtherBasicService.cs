using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Tests.Classes
{
    public class OtherBasicService : IBasicService
    {
        public int Count { get; set; }

        public int Execute()
        {
            return Count;
        }
    }
}
