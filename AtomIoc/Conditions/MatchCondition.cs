using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Interfaces;

namespace AtomIoc.Conditions
{
    public class MatchCondition : ICondition
    {
        private readonly Func<IStrategy, InjectionContext, bool> _match;

        public MatchCondition(Func<IStrategy, InjectionContext, bool> match)
        {
            _match = match;
        }

        public bool MeetsCondition(IStrategy strategy, InjectionContext context)
        {
            return _match(strategy, context);
        }
    }
}
