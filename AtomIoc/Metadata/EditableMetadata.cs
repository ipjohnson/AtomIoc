using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Data;
using AtomIoc.Interfaces;

namespace AtomIoc.Metadata
{
    public class EditableMetadata : IMetadata
    {
        protected ImmutableHashTree<object, object> Values = ImmutableHashTree<object, object>.Empty;

        public bool Has(object key)
        {
            return Values.ContainsKey(key);
        }

        public T GetValueOrDefault<T>(object key, T defaultT = default(T))
        {
            return (T)Values.GetValueOrDefault(key, defaultT);
        }

        public void AddData(object key, object value)
        {
            Values = Values.Add(key, value);
        }
    }
}
