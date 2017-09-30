using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Tests.Classes
{
    public interface IMultipleService
    {
        string Name { get; }
    }

    public class MultipleServiceA : IMultipleService
    {
        public string Name => "A";
    }
    public class MultipleServiceB : IMultipleService
    {
        public string Name => "B";
    }
    public class MultipleServiceC : IMultipleService
    {
        public string Name => "C";
    }
    public class MultipleServiceD : IMultipleService
    {
        public string Name => "D";
    }
    public class MultipleServiceE : IMultipleService
    {
        public string Name => "E";
    }
}
