using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Tests.Classes
{
    public interface IBasicService
    {
        int Count { get; set; }

        int Execute();
    }

    public class BasicService : IBasicService
    {
        public int Count { get; set; }

        public int Execute()
        {
            return Count;
        }
    }
}
