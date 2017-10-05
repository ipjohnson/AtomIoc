using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Interfaces
{
    public interface IConfigurableStrategy : IStrategy
    {
        new ILifestyle Lifestyle { get; set; }

        void AddCondition(ICondition condition);

        void AddMetadata(object key, object value);
    }
}
