using System;
using System.Data.Common;
using Dapper.Logging.Configuration;
using Dapper.Logging.Hooks;
using Microsoft.Extensions.Logging;

namespace Dapper.Logging
{
    internal class ContextlessLoggingFactory : IDbConnectionFactory
    {
        private class Empty { }
        private readonly LoggingHook<Empty> _hooks;
        private readonly WrappedConnectionFactory<Empty> _factory;
        private static readonly Empty Context = new Empty();
        
        public ContextlessLoggingFactory(
            ILogger<DbConnection> logger, 
            DbLoggingConfiguration config, 
            Func<DbConnection> factory)
        {
            _hooks = new LoggingHook<Empty>(logger, config);
            _factory = new WrappedConnectionFactory<Empty>(factory);
        }
        
        public DbConnection CreateConnection() => 
            _factory.CreateConnection(_hooks, Context);
    }
}