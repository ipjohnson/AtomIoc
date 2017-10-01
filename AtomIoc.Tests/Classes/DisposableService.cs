using System;
using System.Collections.Generic;
using System.Text;

namespace AtomIoc.Tests.Classes
{
    public interface IDisposableService : IDisposable
    {
        event EventHandler<EventArgs> Disposed;
    }

    public class DisposableService : IDisposableService
    {
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> Disposed;
    }
}
