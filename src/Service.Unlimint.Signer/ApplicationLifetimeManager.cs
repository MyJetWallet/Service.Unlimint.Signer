using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.NoSql;
using MyJetWallet.Sdk.Service;
using Service.Unlimint.Signer.Jobs;

namespace Service.Unlimint.Signer
{
    public class ApplicationLifetimeManager : ApplicationLifetimeManagerBase
    {
        private readonly ILogger<ApplicationLifetimeManager> _logger;
        private readonly MyNoSqlClientLifeTime _myNoSqlClient;
        private readonly UpdateEncryptionKeysJob _updateEncryptionKeysJob;
        private readonly SetSourceWalletIdJob _setSourceWalletIdJob;

        public ApplicationLifetimeManager(IHostApplicationLifetime appLifetime,
            ILogger<ApplicationLifetimeManager> logger, 
            MyNoSqlClientLifeTime myNoSqlClient,
            UpdateEncryptionKeysJob updateEncryptionKeysJob,
            SetSourceWalletIdJob setSourceWalletIdJob)
            : base(appLifetime)
        {
            _logger = logger;
            _myNoSqlClient = myNoSqlClient;
            _updateEncryptionKeysJob = updateEncryptionKeysJob;
            _setSourceWalletIdJob = setSourceWalletIdJob;
        }

        protected override void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called");
            _myNoSqlClient.Start();
            _logger.LogInformation("MyNoSqlTcpClient is started");
            _updateEncryptionKeysJob.Start();
            _setSourceWalletIdJob.Start();
            _logger.LogInformation("UpdateEncryptionKeysJob is started");
        }

        protected override void OnStopping()
        {
            _logger.LogInformation("OnStopping has been called");
            _myNoSqlClient.Stop();
            _logger.LogInformation("MyNoSqlTcpClient is stopped");
            _updateEncryptionKeysJob.Stop();
            _setSourceWalletIdJob.Stop();
            _logger.LogInformation("UpdateEncryptionKeysJob is stopped");
        }

        protected override void OnStopped()
        {
            _logger.LogInformation("OnStopped has been called");
        }
    }
}