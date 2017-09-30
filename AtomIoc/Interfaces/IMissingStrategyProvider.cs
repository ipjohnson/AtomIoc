using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Data;

namespace AtomIoc.Interfaces
{
    public class StrategyInformation
    {
        public IStrategy Strategy { get; set; }

        public IEnumerable<Type> As { get; set; }

        public IEnumerable<KeyedType> AsKeyed { get; set; }
    }

    public interface IMissingStrategyProvider
    {
        IEnumerable<StrategyInformation> ProvideStrategies(InjectionContext injectionContext);
    }
}
