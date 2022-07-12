using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.ApiSecurityManager.ApiKeys;
using MyJetWallet.Circle;
using MyJetWallet.Circle.Models.Payments;
using MyJetWallet.Sdk.Service;
using Service.PersonalData.Grpc;
using Service.PersonalData.Grpc.Contracts;
using Service.Unlimint.Signer.Grpc;
using Service.Unlimint.Signer.Grpc.Models;

// ReSharper disable InconsistentLogPropertyNaming

namespace Service.Unlimint.Signer.Services
{
    public class UnlimintPaymentsService : IUnlimintPaymentsService
    {
        private readonly CircleClient _circleClient;
        private readonly ILogger<UnlimintPaymentsService> _logger;
        private readonly IPersonalDataServiceGrpc _personalDataServiceGrpc;
        private readonly IApiKeyStorage _apiKeyStorage;

        public UnlimintPaymentsService(
            ILogger<UnlimintPaymentsService> logger,
            IPersonalDataServiceGrpc personalDataServiceGrpc,
            IApiKeyStorage apiKeyStorage
        )
        {
            _logger = logger;
            _personalDataServiceGrpc = personalDataServiceGrpc;
            _apiKeyStorage = apiKeyStorage;
            _circleClient = new CircleClient(null, Program.Settings.CircleNetwork);
        }

        public async Task<Response<UnlimintPaymentInfo>> AddUnlimintPayment(AddPaymentRequest request)
        {
            using var action = MyTelemetry.StartActivity("Create Circle payment");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured for broker {brokerId}",
                        request.BrokerId);
                    return Response<UnlimintPaymentInfo>.Error("Api key is not configured");
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
                    return Response<UnlimintPaymentInfo>.Error("Unable to get personal data for client");
                }

                //_logger.LogInformation("CIRCLE PAYMENT: {context}", (new 
                //{
                //    request = request
                //}).ToJson());

                var response = await _circleClient.CreatePaymentAsync(request.IdempotencyKey, request.KeyId,
                    personalData.PersonalData.Email,
                    string.IsNullOrEmpty(personalData.PersonalData.Phone) ? null : personalData.PersonalData.Phone,
                    request.SessionId,
                    request.IpAddress, request.Amount.ToString(CultureInfo.InvariantCulture), request.Currency,
                    request.Verification, request.SourceId, request.SourceType, request.Description,
                    request.EncryptedData, Program.Settings.Success3dUrl, Program.Settings.Failure3dUrl);

                _logger.LogInformation("Execute CreatePaymentAsync: Request: {requestJson}; Response: {responseJson}",
                    request.ToJson(), response.ToJson());

                return !response.Success
                    ? Response<UnlimintPaymentInfo>.Error(response.Message)
                    : Response<UnlimintPaymentInfo>.Success(new UnlimintPaymentInfo
                    {
                        Id = response.Data.Id,
                        Status = response.Data.Status,
                        TrackingRef = response.Data.TrackingRef,
                        ErrorCode = response.Data.ErrorCode,
                        RedirectUrl = response.Data.RequiredAction?.RedirectUrl
                    });
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                return Response<UnlimintPaymentInfo>.Error(ex.Message);
            }
        }

        public async Task<Response<PaymentInfo>> GetUnlimintPaymentInfo(GetPaymentRequest request)
        {
            using var action = MyTelemetry.StartActivity("Get Circle payment");
            request.AddToActivityAsJsonTag("request");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured for broker {brokerId}",
                        request.BrokerId);
                    return Response<PaymentInfo>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.GetPaymentAsync(request.PaymentId);

                if (!response.Success)
                {
                    _logger.LogError("Error depositing from circle card {context}",
                        (new
                        {
                            request = request,
                            response = response,
                        }).ToJson());
                    return Response<PaymentInfo>.Error(response.Message);
                }

                return Response<PaymentInfo>.Success(response.Data);
            }
            catch (Exception exception)
            {
                return Response<PaymentInfo>.Error(exception.Message);
            }
        }
    }
}