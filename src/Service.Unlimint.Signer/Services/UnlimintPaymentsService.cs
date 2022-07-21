using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Service;
using MyJetWallet.Unlimint;
using MyJetWallet.Unlimint.Models;
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
        private readonly UnlimintClient _unlimintClient;
        private readonly UnlimintAuthClient _unlimintAuthClient;

        private readonly ILogger<UnlimintPaymentsService> _logger;
        private readonly IPersonalDataServiceGrpc _personalDataServiceGrpc;

        public UnlimintPaymentsService(
            ILogger<UnlimintPaymentsService> logger,
            IPersonalDataServiceGrpc personalDataServiceGrpc)
        {
            _logger = logger;
            _personalDataServiceGrpc = personalDataServiceGrpc;
            _unlimintAuthClient = new UnlimintAuthClient(
                Program.Settings.UnlimintTerminalCode,
                Program.Settings.UnlimintPassword,
                Program.Settings.UnlimintNetwork);
            _unlimintClient = new UnlimintClient(null,
                Program.Settings.UnlimintNetwork);
        }

        public async Task<Response<CreatePaymentInfo>> CreateUnlimintPaymentAsync(CreatePaymentRequest request)
        {
            using var action = MyTelemetry.StartActivity("Create Unlimint payment");
            try
            {
                var token = await _unlimintAuthClient.GetAuthorizationTokenAsync();
                _unlimintClient.SetAccessToken(token.Data.AccessToken);

                var personalData = await _personalDataServiceGrpc.GetByIdAsync(new GetByIdRequest
                {
                    Id = request.ClientId
                });

                if (personalData?.PersonalData == null)
                {
                    _logger.LogError("Unable to get personal data for client {clientId}",
                        request.ClientId);
                    return Response<CreatePaymentInfo>.Error("Unable to get personal data for client");
                }

                var response = await _unlimintClient
                    .CreatePaymentAsync(
                        request.MerchantOrderId,
                        request.PaymentId,
                        personalData.PersonalData.Email,
                        string.IsNullOrEmpty(personalData.PersonalData.Phone) ? null : personalData.PersonalData.Phone,
                        request.SessionId,
                        request.IpAddress,
                        request.Amount,
                        request.Currency,
                        request.Description,
                        request.GenerateToken,
                        request.UseThreeDsChallengeIndicator,
                        request.Description,
                        Program.Settings.Success3dUrl,
                        Program.Settings.Failure3dUrl,
                        Program.Settings.Cancel3dUrl,
                        Program.Settings.InProcess3dUrl,
                        Program.Settings.Return3dUrl,
                        DateTime.UtcNow,
                        "BANKCARD",
                        request.ClientId
                    );

                _logger.LogInformation("Execute CreatePaymentAsync: Request: {requestJson}; Response: {responseJson}",
                    request.ToJson(), response.ToJson());

                return !response.Success
                    ? Response<CreatePaymentInfo>.Error(response.Message)
                    : Response<CreatePaymentInfo>.Success(new CreatePaymentInfo
                    {
                        Id = response.Data?.PaymentData?.Id,
                        // TODO: Add status response.Data.Status,
                        //Status = PaymentStatus.New, 
                        // TODO: Add TrackingRef response.Data.TrackingRef
                        //TrackingRef = String.Empty,//response.Data.TrackingRef,
                        // TODO: Add ErrorCode response.Data.TrackingRef
                        //ErrorCode = null,//response.Data.PaymentData.,
                        RedirectUrl = response.Data?.RedirectUrl
                    });
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                return Response<CreatePaymentInfo>.Error(ex.Message);
            }
        }

        public async Task<Response<GetPaymentInfo>> GetUnlimintPaymentByIdAsync(GetPaymentByIdRequest request)
        {
            using var action = MyTelemetry.StartActivity("Get Unlimint payment");
            request.AddToActivityAsJsonTag("get-unlimint-payment-by-id");
            try
            {
                var token = await _unlimintAuthClient.GetAuthorizationTokenAsync();
                _unlimintClient.SetAccessToken(token.Data.AccessToken);

                var response = await _unlimintClient
                    .GetPaymentByIdAsync(request.PaymentId);

                if (!response.Success)
                {
                    _logger.LogError("Error depositing from unlimint card {context}",
                        (new
                        {
                            request = request,
                            response = response,
                        }).ToJson());
                    return Response<GetPaymentInfo>.Error(response.Message);
                }

                var payment = response.Data;
                var data = new GetPaymentInfo();
                data.Id = payment?.PaymentData.Id;
                data.Type = payment?.PaymentMethod;
                data.MerchantOrderId = payment?.MerchantOrder.Id;
                data.Card = new CardDescription
                {
                    AcctType = payment?.CardAccount.AcctType,
                    Expiration = payment?.CardAccount.Expiration,
                    Holder = payment?.CardAccount.Holder,
                    IssuingCountryCode = payment?.CardAccount.IssuingCountryCode,
                    MaskedPan = payment?.CardAccount.MaskedPan,
                    Token = payment?.CardAccount.Token,
                };
                data.MerchantWalletId = payment?.PaymentData.Currency;
                data.Description = payment?.PaymentData.Note;
                data.Status = payment?.PaymentData.Status;
                data.Amount = new UnlimintAmount
                {
                    Amount = payment?.PaymentData.Amount ?? 0m,
                    Currency = payment?.PaymentData.Currency,
                };
                // data.Fee = new UnlimintAmount
                // {
                //     Amount = 0m,
                //     Currency = payment?.PaymentData.Currency,
                // };
                data.TrackingRef = payment?.PaymentData.Rrn;
                data.ErrorCode = payment?.PaymentData.DeclineCode;
                data.Metadata = new Metadata()
                {
                    Email = payment?.Customer.Email,
                    PhoneNumber = payment?.Customer.Phone,
                    SessionId = payment?.Customer.FullName,
                    IpAddress = payment?.Customer.Ip,
                };
                data.CreateDate = payment?.PaymentData.Created;
                return Response<GetPaymentInfo>.Success(data);
            }
            catch (Exception exception)
            {
                return Response<GetPaymentInfo>.Error(exception.Message);
            }
        }

        public async Task<Response<GetPaymentInfo>> GetUnlimintPaymentByMerchantIdAsync(
            GetPaymentByMerchantIdRequest request)
        {
            using var action = MyTelemetry.StartActivity("Get Unlimint payment");
            request.AddToActivityAsJsonTag("get-unlimint-payment-by-merchant-id");
            try
            {
                var token = await _unlimintAuthClient.GetAuthorizationTokenAsync();
                _unlimintClient.SetAccessToken(token.Data.AccessToken);

                var response = await _unlimintClient
                    .GetPaymentByMerchantOrderIdAsync(request.MerchantId, request.MatchingEngineId);

                if (!response.Success)
                {
                    _logger.LogError("Error depositing from unlimint card {context}",
                        (new
                        {
                            request = request,
                            response = response,
                        }).ToJson());
                    return Response<GetPaymentInfo>.Error(response.Message);
                }

                var payment = response.Data.Payments.FirstOrDefault();
                
                var data = new GetPaymentInfo();
                data.Id = payment?.PaymentData.Id;
                data.Type = payment?.PaymentMethod;
                data.MerchantOrderId = payment?.MerchantOrder.Id;
                data.Card = new CardDescription
                {
                    AcctType = payment?.CardAccount.AcctType,
                    Expiration = payment?.CardAccount.Expiration,
                    Holder = payment?.CardAccount.Holder,
                    IssuingCountryCode = payment?.CardAccount.IssuingCountryCode,
                    MaskedPan = payment?.CardAccount.MaskedPan,
                    Token = payment?.CardAccount.Token,
                };
                data.MerchantWalletId = payment?.PaymentData.Currency;
                data.Description = payment?.PaymentData.Note;
                data.Status = payment?.PaymentData.Status;
                data.Amount = new UnlimintAmount
                {
                    Amount = payment?.PaymentData.Amount ?? 0m,
                    Currency = payment?.PaymentData.Currency,
                };
                // data.Fee = new UnlimintAmount
                // {
                //     Amount = 0m,
                //     Currency = payment?.PaymentData.Currency,
                // };
                data.TrackingRef = payment?.PaymentData.Rrn;
                data.ErrorCode = payment?.PaymentData.DeclineCode;
                data.Metadata = new Metadata()
                {
                    Email = payment?.Customer.Email,
                    PhoneNumber = payment?.Customer.Phone,
                    SessionId = payment?.Customer.FullName,
                    IpAddress = payment?.Customer.Ip,
                };
                data.CreateDate = payment?.PaymentData.Created;
                return Response<GetPaymentInfo>.Success(data);
            }
            catch (Exception exception)
            {
                return Response<GetPaymentInfo>.Error(exception.Message);
            }
        }
    }
}