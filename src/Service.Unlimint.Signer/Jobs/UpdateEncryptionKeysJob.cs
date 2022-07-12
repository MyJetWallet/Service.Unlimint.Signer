using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.ApiSecurityManager.ApiKeys;
using MyJetWallet.Circle;
using MyJetWallet.Sdk.Service;
using MyJetWallet.Sdk.Service.Tools;
using Service.Circle.Signer;
using Service.Unlimint.Signer.Services;

// ReSharper disable InconsistentLogPropertyNaming

namespace Service.Unlimint.Signer.Jobs
{
    public class UpdateEncryptionKeysJob : IDisposable
    {
        private readonly CircleClient _circleClient;

        private readonly ILogger<UpdateEncryptionKeysJob> _logger;
        private readonly IApiKeyStorage _apiKeyStorage;
        private readonly RsaKeyStorage _rsaKeyStorage;
        private readonly MyTaskTimer _timer;

        public UpdateEncryptionKeysJob(ILogger<UpdateEncryptionKeysJob> logger,
            IApiKeyStorage apiKeyStorage,
            RsaKeyStorage rsaKeyStorage)
        {
            _logger = logger;
            _apiKeyStorage = apiKeyStorage;
            _rsaKeyStorage = rsaKeyStorage;
            _circleClient = new CircleClient(null, Program.Settings.CircleNetwork);

            _timer = new MyTaskTimer(typeof(UpdateEncryptionKeysJob),
                TimeSpan.FromSeconds(Program.Settings.EncryptionKeysUpdateIntervalSec),
                logger, DoTime);
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
                    _logger.LogInformation("Get circle key {url} [key: {key}]", _circleClient.EndpointUrl,
                        $"{apiKey.ApiKeyValue.Substring(0,5)}...{apiKey.ApiKeyValue.Substring(apiKey.ApiKeyValue.Length-5,5)}");
                    var key = await _circleClient.GetPublicKeyAsync();
                    _logger.LogInformation("Get circle key {url} - {key}", _circleClient.EndpointUrl, key.ToJson());
                    if (key.Success)
                    {
                        _rsaKeyStorage.SetKey(key.Data.KeyId, new RsaKeyEntry
                        {
                            PubKey = key.Data.RsaPublicKey,
                            UpdateDate = DateTime.UtcNow
                        });

                        break;
                    }
                    else
                    {
                        _logger.LogError("Unable to update encryption key for broker: {error}", key.Message);
                        continue;
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogError("Unable to update encryption key for broker: {error}", exception.Message);
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