using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Data;

namespace AtomIoc.Interfaces
{
    public interface IMetadata
    {
        bool Has(object key);

        T GetValueOrDefault<T>(object key, T defaultT = default(T));
    }
}
