using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Interfaces;

namespace AtomIoc
{
    public class Meta<T>
    {
        public Meta(T value, IMetadata metadata)
        {
            Value = value;
            Metadata = metadata;
        }

        public T Value { get; }

        public IMetadata Metadata { get; }
    }
}
