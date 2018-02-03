using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Tests.Classes
{
    public interface IGenericService<T>
    {
        T Value { get; }
    }

    public class GenericService<T> : IGenericService<T>
    {
        public GenericService(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }
}
