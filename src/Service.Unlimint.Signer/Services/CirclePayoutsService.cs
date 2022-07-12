using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.ApiSecurityManager.ApiKeys;
using MyJetWallet.Circle;
using MyJetWallet.Circle.Models.Payouts;
using MyJetWallet.Sdk.Service;
using Service.Circle.Signer;
using Service.PersonalData.Grpc;
using Service.PersonalData.Grpc.Contracts;
using Service.Unlimint.Signer.Grpc;
using Service.Unlimint.Signer.Grpc.Models;
using Service.Unlimint.Signer.Grpc.Models.BusinessAccount;
using Service.Unlimint.Signer.Settings;

// ReSharper disable InconsistentLogPropertyNaming

namespace Service.Unlimint.Signer.Services
{
    public class CirclePayoutsService : ICirclePayoutsService
    {
        private readonly CircleClient _circleClient;
        private readonly ILogger<CirclePayoutsService> _logger;
        private readonly IPersonalDataServiceGrpc _personalDataServiceGrpc;
        private readonly IApiKeyStorage _apiKeyStorage;

        public CirclePayoutsService(
            ILogger<CirclePayoutsService> logger,
            IPersonalDataServiceGrpc personalDataServiceGrpc,
            IApiKeyStorage apiKeyStorage)
        {
            _logger = logger;
            _personalDataServiceGrpc = personalDataServiceGrpc;
            _apiKeyStorage = apiKeyStorage;
            _circleClient = new CircleClient(null, Program.Settings.CircleNetwork);
        }

        public async Task<Response<CirclePayoutInfo>> AddCirclePayout(AddPayoutRequest request)
        {
            using var action = MyTelemetry.StartActivity("Create Circle Payout");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured for broker {brokerId}",
                        request.BrokerId);
                    return Response<CirclePayoutInfo>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var personalData = await _personalDataServiceGrpc.GetByIdAsync(new GetByIdRequest
                {
                    Id = request.ClientId
                });

                if (personalData?.PersonalData == null)
                {
                    _logger.LogError("Unable to get personal data for client {clientId}",
                        request.ClientId);
                    return Response<CirclePayoutInfo>.Error("Unable to get personal data for client");
                }

                var response = await _circleClient.CreatePayoutAsync(request.IdempotencyKey,
                    request.Amount.ToString(CultureInfo.InvariantCulture),
                    request.Currency, CircleBusinessWallet.WalletId, request.DestinationId, request.DestinationType, 
                    personalData.PersonalData.Email);

                return !response.Success
                    ? Response<CirclePayoutInfo>.Error(response.Message)
                    : Response<CirclePayoutInfo>.Success(new CirclePayoutInfo
                    {
                        Id = response.Data.Id,
                        Status = response.Data.Status,
                        TrackingRef = response.Data.TrackingRef,
                        ErrorCode = response.Data.Error,
                    });
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                return Response<CirclePayoutInfo>.Error(ex.Message);
            }
        }

        public async Task<Response<PayoutInfo>> GetCirclePayoutInfo(GetPayoutRequest request)
        {
            using var action = MyTelemetry.StartActivity("Create Circle Payout");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured for broker {brokerId}",
                        request.BrokerId);
                    return Response<PayoutInfo>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.GetPayoutAsync(request.PayoutId);

                return !response.Success
                    ? Response<PayoutInfo>.Error(response.Message)
                    : Response<PayoutInfo>.Success(response.Data);
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                return Response<PayoutInfo>.Error(ex.Message);
            }
        }

        public async Task<Response<PayoutInfo[]>> GetCirclePayoutsInfo(CirclePaginationRequest request)
        {
            using var action = MyTelemetry.StartActivity("Get Circle Payout");
            request.AddToActivityAsJsonTag("request");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured");
                    return Response<PayoutInfo[]>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.GetPayoutsAsync(request.TakeAfter, request.PageSize);

                return !response.Success
                    ? Response<PayoutInfo[]>.Error(response.Message)
                    : Response<PayoutInfo[]>.Success(response.Data);
            }
            catch (Exception exception)
            {
                return Response<PayoutInfo[]>.Error(exception.Message);
            }
        }
    }
}