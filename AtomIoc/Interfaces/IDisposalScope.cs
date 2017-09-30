using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Interfaces
{
    /// <summary>
    /// Represents a scope that holds disposable object
    /// </summary>
    public interface IDisposalScope : IDisposable
    {
        /// <summary>
        /// Add an object for disposal tracking
        /// </summary>
        /// <param name="disposable">object to track for disposal</param>
        object AddDisposable(object disposable);

        /// <summary>
        /// Add an object for disposal tracking
        /// </summary>
        /// <param name="disposable">object to track for disposal</param>
        /// <param name="cleanupDelegate">logic that will be run directly before the object is disposed</param>
        object AddDisposable(object disposable, Action<object> cleanupDelegate);
    }
}
