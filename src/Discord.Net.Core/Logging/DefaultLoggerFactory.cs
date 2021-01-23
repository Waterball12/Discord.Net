using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Logging
{
    internal class DefaultLoggerFactory : ILoggerFactory
    {
        private List<ILoggerProvider> Providers { get; } = new List<ILoggerProvider>();
        private bool _isDisposed = false;

        public void Dispose()
        {
            if (this._isDisposed)
                return;
            this._isDisposed = true;

            foreach (var provider in this.Providers)
                provider.Dispose();

            this.Providers.Clear();
        }

        public ILogger CreateLogger(string categoryName)
        {
            if (this._isDisposed)
                throw new InvalidOperationException("This logger factory is already disposed.");

            // HEHEHE XDXD
            var provider = Providers[0];

            return provider.CreateLogger(categoryName);
        }

        public void AddProvider(ILoggerProvider provider)
        {
            this.Providers.Add(provider);
        }
    }
}
