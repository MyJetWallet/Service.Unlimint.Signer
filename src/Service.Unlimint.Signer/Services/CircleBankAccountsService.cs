using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.ApiSecurityManager.ApiKeys;
using MyJetWallet.Circle;
using MyJetWallet.Sdk.Service;
using Service.Circle.Signer;
using Service.Unlimint.Signer.Grpc;
using Service.Unlimint.Signer.Grpc.Models;

// ReSharper disable InconsistentLogPropertyNaming

namespace Service.Unlimint.Signer.Services
{
    public class CircleBankAccountsService : ICircleBankAccountsService
    {
        private readonly CircleClient _circleClient;
        private readonly ILogger<CircleBankAccountsService> _logger;
        private readonly IApiKeyStorage _apiKeyStorage;

        public CircleBankAccountsService(ILogger<CircleBankAccountsService> logger,
            IApiKeyStorage apiKeyStorage)
        {
            _logger = logger;
            _apiKeyStorage = apiKeyStorage;

            _circleClient = new CircleClient(null, Program.Settings.CircleNetwork);
        }

        public async Task<Response<CircleBankAccountDetails>> AddCircleSepaBankAccount(AddSepaBankAccountRequest bankAccount)
        {
            using var action = MyTelemetry.StartActivity("Add Circle Sepa BankAccount");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured for broker {brokerId}",
                        bankAccount.BrokerId);
                    return Response<CircleBankAccountDetails>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.CreateBankAccountSepaAsync(bankAccount.IdempotencyKey,
                    bankAccount.Iban,
                    bankAccount.BillingDetails,
                    bankAccount.BankAddress);

                if (!response.Success) return Response<CircleBankAccountDetails>.Error(response.Message, response.ResponseStatusCode != null ? (int)response.ResponseStatusCode.Value : 500);

                return Response<CircleBankAccountDetails>.Success(
                    new CircleBankAccountDetails
                    {
                        Id = response.Data.Id,
                        Status = response.Data.Status,
                        BankAddress = response.Data.BankAddress,
                        BillingDetails = response.Data.BillingDetails,
                        Description = response.Data.Description,
                        Fingerprint = response.Data.Fingerprint,
                        TrackingRef = response.Data.TrackingRef,
                        CreateDate = response.Data.CreateDate.UtcDateTime,
                        UpdateDate = response.Data.UpdateDate.UtcDateTime
                    });
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                return Response<CircleBankAccountDetails>.Error(ex.Message);
            }
        }

        public async Task<Response<CircleBankAccountDetails>> AddCircleUsSwiftBankAccount(AddUsSwiftBankAccountRequest bankAccount)
        {
            using var action = MyTelemetry.StartActivity("Add Circle Sepa BankAccount");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured for broker {brokerId}",
                        bankAccount.BrokerId);
                    return Response<CircleBankAccountDetails>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.CreateBankAccountUsSwiftAsync(bankAccount.IdempotencyKey,
                    bankAccount.AccountNumber,
                    bankAccount.RoutingNumber,
                    bankAccount.BillingDetails,
                    bankAccount.BankAddress);

                if (!response.Success) return Response<CircleBankAccountDetails>.Error(response.Message,
                    response.ResponseStatusCode != null ? (int)response.ResponseStatusCode.Value : 500);

                return Response<CircleBankAccountDetails>.Success(
                    new CircleBankAccountDetails
                    {
                        Id = response.Data.Id,
                        Status = response.Data.Status,
                        BankAddress = response.Data.BankAddress,
                        BillingDetails = response.Data.BillingDetails,
                        Description = response.Data.Description,
                        Fingerprint = response.Data.Fingerprint,
                        TrackingRef = response.Data.TrackingRef,
                        CreateDate = response.Data.CreateDate.UtcDateTime,
                        UpdateDate = response.Data.UpdateDate.UtcDateTime
                    });
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                return Response<CircleBankAccountDetails>.Error(ex.Message);
            }
        }

        public async Task<Response<CircleBankWireTransferDetails>> GetBankWireTransferDetails(GetBankWireTransferDetailsRequest request)
        {
            using var action = MyTelemetry.StartActivity("GetBankWireTransferDetails");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured for broker {brokerId}",
                        request.BrokerId);
                    return Response<CircleBankWireTransferDetails>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.ObtainBankWireTransferDetailsAsync(request.BankAccountId);

                if (!response.Success) return Response<CircleBankWireTransferDetails>.Error(response.Message);

                return Response<CircleBankWireTransferDetails>.Success(
                    new CircleBankWireTransferDetails
                    {
                        Beneficiary = response.Data.Beneficiary,
                        BeneficiaryBank = response.Data.BeneficiaryBank,
                        TrackingRef = $"{response.Data.TrackingRef}*Simple",
                    });
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                return Response<CircleBankWireTransferDetails>.Error(ex.Message);
            }
        }
    }
}