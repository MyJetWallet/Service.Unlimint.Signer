using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.ApiSecurityManager.ApiKeys;
using MyJetWallet.Circle;
using MyJetWallet.Circle.Models.ChargeBacks;
using MyJetWallet.Sdk.Service;
using Service.Circle.Signer;
using Service.PersonalData.Grpc;
using Service.Unlimint.Signer.Grpc;
using Service.Unlimint.Signer.Grpc.Models;
using Service.Unlimint.Signer.Grpc.Models.BusinessAccount;

// ReSharper disable InconsistentLogPropertyNaming

namespace Service.Unlimint.Signer.Services
{
    public class CircleChargebacksService : ICircleChargebacksService
    {
        private readonly CircleClient _circleClient;
        private readonly ILogger<CircleChargebacksService> _logger;
        private readonly IPersonalDataServiceGrpc _personalDataServiceGrpc;
        private readonly IApiKeyStorage _apiKeyStorage;

        public CircleChargebacksService(
            ILogger<CircleChargebacksService> logger,
            IPersonalDataServiceGrpc personalDataServiceGrpc,
            IApiKeyStorage apiKeyStorage
            )
        {
            _logger = logger;
            _personalDataServiceGrpc = personalDataServiceGrpc;
            _apiKeyStorage = apiKeyStorage;
            _circleClient = new CircleClient(null, Program.Settings.CircleNetwork);
        }

        public async Task<Response<Chargeback[]>> GetChargebacks(CirclePaginationRequest request)
        {
            using var action = MyTelemetry.StartActivity("GetChargebacks");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured");
                    return Response<Chargeback[]>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.GetChargebacksAsync(
                    request.TakeAfter,
                    request.PageSize);

                return !response.Success
                    ? Response<Chargeback[]>.Error(response.Message)
                    : Response<Chargeback[]>.Success(response.Data);
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                _logger.LogError(ex, "GetChargebacks");
                return Response<Chargeback[]>.Error(ex.Message);
            }
        }

    }
}