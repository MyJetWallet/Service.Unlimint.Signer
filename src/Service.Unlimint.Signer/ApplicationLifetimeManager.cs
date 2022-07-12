using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.NoSql;
using MyJetWallet.Sdk.Service;

namespace Service.Unlimint.Signer
{
    public class ApplicationLifetimeManager : ApplicationLifetimeManagerBase
    {
        private readonly ILogger<ApplicationLifetimeManager> _logger;
        private readonly MyNoSqlClientLifeTime _myNoSqlClient;

        public ApplicationLifetimeManager(IHostApplicationLifetime appLifetime,
            ILogger<ApplicationLifetimeManager> logger, 
            MyNoSqlClientLifeTime myNoSqlClient)
            : base(appLifetime)
        {
            _logger = logger;
            _myNoSqlClient = myNoSqlClient;
        }

        protected override void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called");
            _myNoSqlClient.Start();
            _logger.LogInformation("MyNoSqlTcpClient is started");
            _logger.LogInformation("UpdateEncryptionKeysJob is started");
        }

        protected override void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called");
            _myNoSqlClient.Stop();
            _logger.LogInformation("MyNoSqlTcpClient is stopped");
            _logger.LogInformation("UpdateEncryptionKeysJob is stopped");
        }

        protected override void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called");
        }
    }
}