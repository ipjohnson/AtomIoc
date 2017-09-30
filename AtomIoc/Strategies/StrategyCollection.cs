using System;
using System.Collections.Generic;
using System.Text;
using AtomIoc.Data;
using AtomIoc.Interfaces;

namespace AtomIoc.Strategies
{
    public class StrategyCollection
    {
        private ImmutableLinkedList<IStrategy> _strategies = ImmutableLinkedList<IStrategy>.Empty;

        public ImmutableLinkedList<IStrategy> Strategies => _strategies;

        public void AddStrategy(IStrategy strategy) => ImmutableLinkedList.ThreadSafeAdd(ref _strategies, strategy);
    }
}
