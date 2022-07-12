using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.ApiSecurityManager.ApiKeys;
using MyJetWallet.Sdk.Service;
using MyJetWallet.Unlimint;
using MyJetWallet.Unlimint.Models.Payments;
using Service.PersonalData.Grpc;
using Service.PersonalData.Grpc.Contracts;
using Service.Unlimint.Signer.Grpc;
using Service.Unlimint.Signer.Grpc.Models;

// ReSharper disable InconsistentLogPropertyNaming

namespace Service.Unlimint.Signer.Services
{
    public class UnlimintPaymentsService : IUnlimintPaymentsService
    {
        private readonly UnlimintClient _unlimitClient;
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
            _unlimitClient = new UnlimintClient(null, Program.Settings.UnlimintNetwork);
        }

        public async Task<Response<UnlimintPaymentInfo>> AddUnlimintPayment(AddPaymentRequest request)
        {
            using var action = MyTelemetry.StartActivity("Create Circle payment");
            try
            {
                // var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                // if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                // {
                //     _logger.LogError("Api key is not configured for broker {brokerId}",
                //         request.BrokerId);
                //     return Response<UnlimintPaymentInfo>.Error("Api key is not configured");
                // }

                //_unlimitClient.SetAccessToken(apiKey.ApiKeyValue);

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

                //_logger.LogInformation("UNLIMINT PAYMENT: {context}", (new 
                //{
                //    request = request
                //}).ToJson());

                var response = await _unlimitClient
                    .CreatePaymentAsync(
                        request.MerchantId,
                        request.PaymentId,
                        personalData.PersonalData.Email,
                        string.IsNullOrEmpty(personalData.PersonalData.Phone) ? null : personalData.PersonalData.Phone,
                        request.SessionId,
                        request.IpAddress,
                        request.Amount,
                        request.Currency,
                        request.Verification,
                        request.SourceId,
                        request.SourceType,
                        request.Description,
                        request.EncryptedData,
                        Program.Settings.Success3dUrl,
                        Program.Settings.Failure3dUrl,
                        DateTime.UtcNow,
                        "BANKCARD"
                    );

                _logger.LogInformation("Execute CreatePaymentAsync: Request: {requestJson}; Response: {responseJson}",
                    request.ToJson(), response.ToJson());

                return !response.Success
                    ? Response<UnlimintPaymentInfo>.Error(response.Message)
                    : Response<UnlimintPaymentInfo>.Success(new UnlimintPaymentInfo
                    {
                        Id = response.Data.PaymentData.Id,
                        // TODO: Add status response.Data.Status,
                        Status = PaymentStatus.Pending, 
                        // TODO: Add TrackingRef response.Data.TrackingRef
                        TrackingRef = String.Empty,//response.Data.TrackingRef,
                        // TODO: Add ErrorCode response.Data.TrackingRef
                        ErrorCode = null,//response.Data.PaymentData.,
                        RedirectUrl = response.Data.RedirectUrl
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
                // var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                // if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                // {
                //     _logger.LogError("Api key is not configured for broker {brokerId}",
                //         request.BrokerId);
                //     return Response<PaymentInfo>.Error("Api key is not configured");
                // }
                //
                // _unlimitClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _unlimitClient
                    .GetPaymentAsync(request.PaymentId);

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

                //TODO: Ask Unliminit - why we receive a lot of payments
                var payment = response.Data.Payments.FirstOrDefault();
                return Response<PaymentInfo>.Success(new PaymentInfo
                {
                    Id = payment?.PaymentData.Id,
                    Type = null,
                    MerchantId = payment?.MerchantOrder.Id,
                    MerchantWalletId = payment?.PaymentData.Currency,
                    Description =null,
                    Status = PaymentStatus.Pending,
                    Captured = false,
                    CaptureDate = null,
                    TrackingRef = null,
                    ErrorCode = null, //payment?.PaymentData.DeclineCode,
                    Metadata = null,
                    RiskEvaluation = null,
                    CreateDate = payment?.PaymentData.Created,
                    UpdateDate = null,
                    TransactionHash = null,
                    RequiredAction = null
                });
            }
            catch (Exception exception)
            {
                return Response<PaymentInfo>.Error(exception.Message);
            }
        }
    }
}