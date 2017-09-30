using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AtomIoc.Utilities
{
    /// <summary>
    /// Unique Id generator
    /// </summary>
    public static class UniqueId
    {
        private static int _count;

        /// <summary>
        /// Post fix for all generated id
        /// </summary>
        public static string Postfix { get; set; } = "-|";

        /// <summary>
        /// Generate unique id for application space
        /// </summary>
        /// <returns></returns>
        public static string Generate()
        {
            return Interlocked.Increment(ref _count) + Postfix;
        }
    }
}
