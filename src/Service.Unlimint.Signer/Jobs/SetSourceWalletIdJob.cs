using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.ApiSecurityManager.ApiKeys;
using MyJetWallet.Circle;
using MyJetWallet.Sdk.Service.Tools;
using Service.Circle.Signer;
using Service.Unlimint.Signer.Settings;

// ReSharper disable InconsistentLogPropertyNaming

namespace Service.Unlimint.Signer.Jobs
{
    public class SetSourceWalletIdJob : IDisposable
    {
        private readonly CircleClient _circleClient;

        private readonly ILogger<SetSourceWalletIdJob> _logger;
        private readonly IApiKeyStorage _apiKeyStorage;
        private readonly MyTaskTimer _timer;


        public SetSourceWalletIdJob(ILogger<SetSourceWalletIdJob> logger,
            IApiKeyStorage apiKeyStorage)
        {
            _logger = logger;
            _apiKeyStorage = apiKeyStorage;
            _circleClient = new CircleClient(null, Program.Settings.CircleNetwork);

            _timer = new MyTaskTimer(typeof(SetSourceWalletIdJob),
                TimeSpan.MaxValue,
                logger, 
                DoTime);
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private async Task DoTime()
        {
            var now = DateTime.Now;
            ApiKey apiKey;
            do
            {
                await Task.Delay(5000);
                apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);

                if (apiKey == null)
                {
                    _logger.LogError("Unable to update encryption key for Circle: api key is not set");
                    continue;
                }

                try
                {
                    _circleClient.SetAccessToken(apiKey.ApiKeyValue);
                    _logger.LogInformation("Get CircleBusinessWallet.WalletId");
                    var config = await _circleClient.GetConfigurationAsync();
                    if (config.Success)
                    {
                        CircleBusinessWallet.WalletId = config.Data.PaymentsConfiguration.MasterWalletId;

                        break;
                    }
                    else
                    {
                        _logger.LogError("Unable to update CircleBusinessWallet.WalletId: {error}", config.Message);
                        continue;
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogError("Unable to update CircleBusinessWallet.WalletId: {error}", exception.Message);
                    continue;
                }
            }
            while (true);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}