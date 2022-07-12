using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.ApiSecurityManager.ApiKeys;
using MyJetWallet.Circle;
using MyJetWallet.Circle.Models.Transfers;
using MyJetWallet.Sdk.Service;
using Service.Circle.Signer;
using Service.PersonalData.Grpc;
using Service.PersonalData.Grpc.Contracts;
using Service.Unlimint.Signer.Grpc;
using Service.Unlimint.Signer.Grpc.Models;

// ReSharper disable InconsistentLogPropertyNaming

namespace Service.Unlimint.Signer.Services
{
    public class CircleTransfersService : ICircleTransfersService
    {
        private readonly CircleClient _circleClient;
        private readonly ILogger<CircleTransfersService> _logger;
        private readonly IPersonalDataServiceGrpc _personalDataServiceGrpc;
        private readonly IApiKeyStorage _apiKeyStorage;

        public CircleTransfersService(
            ILogger<CircleTransfersService> logger,
            IPersonalDataServiceGrpc personalDataServiceGrpc,
            IApiKeyStorage apiKeyStorage
            )
        {
            _logger = logger;
            _personalDataServiceGrpc = personalDataServiceGrpc;
            _apiKeyStorage = apiKeyStorage;
            _circleClient = new CircleClient(null, Program.Settings.CircleNetwork);
        }

        public async Task<Response<TransferInfo>> AddCircleTransfer(AddTransferRequest request)
        {
            using var action = MyTelemetry.StartActivity("Create Circle transfer");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured for broker {brokerId}",
                        request.BrokerId);
                    return Response<TransferInfo>.Error("Api key is not configured");
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
                    return Response<TransferInfo>.Error("Unable to get personal data for client");
                }

                var response = await _circleClient.CreateTransferV2Async(request.IdempotencyKey, request.Amount.ToString(CultureInfo.InvariantCulture), request.Currency, request.SourceId,
                     request.DstAddress, request.DstAddressTag, request.DstChain);

                return !response.Success
                    ? Response<TransferInfo>.Error(response.Message)
                    : Response<TransferInfo>.Success(response.Data);
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                return Response<TransferInfo>.Error(ex.Message);
            }
        }

        public async Task<Response<TransferInfo>> GetCircleTransferInfo(GetTransfersV2Request request)
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
                    return Response<TransferInfo>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.GetTransferV2Async(request.TrasferId);

                return !response.Success
                    ? Response<TransferInfo>.Error(response.Message)
                    : Response<TransferInfo>.Success(response.Data);
            }
            catch (Exception exception)
            {
                return Response<TransferInfo>.Error(exception.Message);
            }
        }

        public async Task<Response<TransferInfo[]>> GetCircleTransfersInfo(GetTransfersRequest request)
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
                    return Response<TransferInfo[]>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.GetTransfersV2Async(request.LastTransferId, request.PageSize);

                return !response.Success
                    ? Response<TransferInfo[]>.Error(response.Message)
                    : Response<TransferInfo[]>.Success(response.Data);
            }
            catch (Exception exception)
            {
                return Response<TransferInfo[]>.Error(exception.Message);
            }
        }
    }
}