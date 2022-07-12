using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.ApiSecurityManager.ApiKeys;
using MyJetWallet.Circle;
using MyJetWallet.Circle.Models.Wallets;
using MyJetWallet.Sdk.Service;
using Service.Circle.Signer;
using Service.PersonalData.Grpc;
using Service.PersonalData.Grpc.Contracts;
using Service.Unlimint.Signer.Grpc.Models;

// ReSharper disable InconsistentLogPropertyNaming

namespace Service.Unlimint.Signer.Services
{
    public class CircleWalletsService
    {
        private readonly CircleClient _circleClient;
        private readonly ILogger<CircleWalletsService> _logger;
        private readonly IPersonalDataServiceGrpc _personalDataServiceGrpc;
        private readonly IApiKeyStorage _apiKeyStorage;

        public CircleWalletsService(
            ILogger<CircleWalletsService> logger,
            IPersonalDataServiceGrpc personalDataServiceGrpc,
            IApiKeyStorage apiKeyStorage
            )
        {
            _logger = logger;
            _personalDataServiceGrpc = personalDataServiceGrpc;
            _apiKeyStorage = apiKeyStorage;
            _circleClient = new CircleClient(null, Program.Settings.CircleNetwork);
        }

        public async Task<Response<WalletInfo>> AddCircleWallet(AddWalletRequest request)
        {
            using var action = MyTelemetry.StartActivity("Create Circle transfer");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured for broker {brokerId}",
                        request.BrokerId);
                    return Response<WalletInfo>.Error("Api key is not configured");
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
                    return Response<WalletInfo>.Error("Unable to get personal data for client");
                }

                var response = await _circleClient.CreateWalletAsync(request.IdempotencyKey, request.Description);

                return !response.Success
                    ? Response<WalletInfo>.Error(response.Message)
                    : Response<WalletInfo>.Success(response.Data);
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                return Response<WalletInfo>.Error(ex.Message);
            }
        }

        public async Task<Response<WalletInfo>> GetCircleWalletInfo(GetWalletRequest request)
        {
            using var action = MyTelemetry.StartActivity("Get Circle transfer");
            request.AddToActivityAsJsonTag("request");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured for broker {brokerId}",
                        request.BrokerId);
                    return Response<WalletInfo>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.GetWalletAsync(request.WalletId);

                return !response.Success
                    ? Response<WalletInfo>.Error(response.Message)
                    : Response<WalletInfo>.Success(response.Data);
            }
            catch (Exception exception)
            {
                return Response<WalletInfo>.Error(exception.Message);
            }
        }

        public async Task<Response<WalletInfo[]>> GetCircleWalletsInfo(GetTransfersRequest request)
        {
            using var action = MyTelemetry.StartActivity("Get Circle transfer");
            request.AddToActivityAsJsonTag("request");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured for broker {brokerId}",
                        request.BrokerId);
                    return Response<WalletInfo[]>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.GetWalletsAsync(request.LastTransferId, request.PageSize);

                return !response.Success
                    ? Response<WalletInfo[]>.Error(response.Message)
                    : Response<WalletInfo[]>.Success(response.Data);
            }
            catch (Exception exception)
            {
                return Response<WalletInfo[]>.Error(exception.Message);
            }
        }
    }
}