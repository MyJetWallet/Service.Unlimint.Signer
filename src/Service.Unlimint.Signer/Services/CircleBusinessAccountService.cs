using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.ApiSecurityManager.ApiKeys;
using MyJetWallet.Circle;
using MyJetWallet.Circle.Models.BusinessAccounts;
using MyJetWallet.Sdk.Service;
using Service.Circle.Signer;
using Service.PersonalData.Grpc;
using Service.Unlimint.Signer.Grpc;
using Service.Unlimint.Signer.Grpc.Models;
using Service.Unlimint.Signer.Grpc.Models.BusinessAccount;

// ReSharper disable InconsistentLogPropertyNaming

namespace Service.Unlimint.Signer.Services
{
    public class CircleBusinessAccountService : ICircleBusinessAccountService
    {
        private readonly CircleClient _circleClient;
        private readonly ILogger<CircleBusinessAccountService> _logger;
        private readonly IPersonalDataServiceGrpc _personalDataServiceGrpc;
        private readonly IApiKeyStorage _apiKeyStorage;

        public CircleBusinessAccountService(
            ILogger<CircleBusinessAccountService> logger,
            IPersonalDataServiceGrpc personalDataServiceGrpc,
            IApiKeyStorage apiKeyStorage
            )
        {
            _logger = logger;
            _personalDataServiceGrpc = personalDataServiceGrpc;
            _apiKeyStorage = apiKeyStorage;
            _circleClient = new CircleClient(null, Program.Settings.CircleNetwork);
        }

        public async Task<Response<ConfigurationInfo>> GetConfigurationInfo()
        {
            using var action = MyTelemetry.StartActivity("GetConfigurationInfo");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured");
                    return Response<ConfigurationInfo>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.GetConfigurationInfoAsync();

                return !response.Success
                    ? Response<ConfigurationInfo>.Error(response.Message)
                    : Response<ConfigurationInfo>.Success(response.Data);
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                _logger.LogError(ex, "GetConfigurationInfo");
                return Response<ConfigurationInfo>.Error(ex.Message);
            }
        }

        public async Task<Response<BalanceInfo>> GetBalances()
        {
            using var action = MyTelemetry.StartActivity("GetBalances");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured");
                    return Response<BalanceInfo>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.GetBalanceAsync();

                return !response.Success
                    ? Response<BalanceInfo>.Error(response.Message)
                    : Response<BalanceInfo>.Success(response.Data);
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                _logger.LogError(ex, "BalanceInfo");
                return Response<BalanceInfo>.Error(ex.Message);
            }
        }

        public async Task<Response<TransferInfo>> CreateTransfer(CreateBusinessTransferRequest request)
        {
            using var action = MyTelemetry.StartActivity("CreateTransfer");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured");
                    return Response<TransferInfo>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.CreateBusinessTransferAsync(
                    request.IdempotencyKey,
                    request.DestinationId,
                    request.Amount,
                    request.Currency);

                return !response.Success
                    ? Response<TransferInfo>.Error(response.Message)
                    : Response<TransferInfo>.Success(response.Data);
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                _logger.LogError(ex, "CreateTransfer");
                return Response<TransferInfo>.Error(ex.Message);
            }
        }

        public async Task<Response<TransferInfo[]>> GetTransfers(CirclePaginationRequest request)
        {
            using var action = MyTelemetry.StartActivity("CreateTransfer");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured");
                    return Response<TransferInfo[]>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.GetBusinessTransferAsync(
                    request.TakeAfter,
                    request.PageSize);

                return !response.Success
                    ? Response<TransferInfo[]>.Error(response.Message)
                    : Response<TransferInfo[]>.Success(response.Data);
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                _logger.LogError(ex, "CreateTransfer");
                return Response<TransferInfo[]>.Error(ex.Message);
            }
        }

        public async Task<Response<DepositAddressInfo>> CreateDepositAddress(CreateDepositAddressRequest request)
        {
            using var action = MyTelemetry.StartActivity("CreateDepositAddress");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured");
                    return Response<DepositAddressInfo>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.CreateBusinessDepositAddressesAsync(
                    request.IdempotencyKey,
                    request.Currency,
                    request.Chain);

                return !response.Success
                    ? Response<DepositAddressInfo>.Error(response.Message)
                    : Response<DepositAddressInfo>.Success(response.Data);
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                _logger.LogError(ex, "CreateDepositAddress");
                return Response<DepositAddressInfo>.Error(ex.Message);
            }
        }

        public async Task<Response<DepositAddressInfo[]>> GetDepositAddresses()
        {
            using var action = MyTelemetry.StartActivity("GetDepositAddresses");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured");
                    return Response<DepositAddressInfo[]>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.GetBusinessDepositAddressesAsync();

                return !response.Success
                    ? Response<DepositAddressInfo[]>.Error(response.Message)
                    : Response<DepositAddressInfo[]>.Success(response.Data);
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                _logger.LogError(ex, "GetDepositAddresses");
                return Response<DepositAddressInfo[]>.Error(ex.Message);
            }
        }

        public async Task<Response<RecipientAddressInfo>> CreateRecipientAddress(CreateBusinessRecipientAddressRequest request)
        {
            using var action = MyTelemetry.StartActivity("CreateRecipientAddress");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured");
                    return Response<RecipientAddressInfo>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.CreateBusinessRecipientAddressAsync(
                    request.IdempotencyKey,
                    request.Currency,
                    request.Chain,
                    request.Address,
                    request.AddressTag,
                    request.Description);

                return !response.Success
                    ? Response<RecipientAddressInfo>.Error(response.Message)
                    : Response<RecipientAddressInfo>.Success(response.Data);
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                _logger.LogError(ex, "CreateRecipientAddress");
                return Response<RecipientAddressInfo>.Error(ex.Message);
            }
        }

        public async Task<Response<RecipientAddressInfo[]>> GetBusinessRecipientAddresses(CirclePaginationRequest request)
        {
            using var action = MyTelemetry.StartActivity("GetBusinessRecipientAddresses");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured");
                    return Response<RecipientAddressInfo[]>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.GetBusinessRecipientAddressesAsync(
                    request.TakeAfter,
                    request.PageSize);

                return !response.Success
                    ? Response<RecipientAddressInfo[]>.Error(response.Message)
                    : Response<RecipientAddressInfo[]>.Success(response.Data);
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                _logger.LogError(ex, "GetBusinessRecipientAddresses");
                return Response<RecipientAddressInfo[]>.Error(ex.Message);
            }
        }

        public async Task<Response<DepositInfo[]>> GetBusinessDeposits(CirclePaginationRequest request)
        {
            using var action = MyTelemetry.StartActivity("GetBusinessDeposits");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured");
                    return Response<DepositInfo[]>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.GetBusinessDepositsAsync(
                    request.TakeAfter,
                    request.PageSize);

                return !response.Success
                    ? Response<DepositInfo[]>.Error(response.Message)
                    : Response<DepositInfo[]>.Success(response.Data);
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                _logger.LogError(ex, "GetBusinessDeposits");
                return Response<DepositInfo[]>.Error(ex.Message);
            }
        }

    }
}