using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.ApiSecurityManager.ApiKeys;
using MyJetWallet.Circle;
using MyJetWallet.Circle.Settings.Services;
using MyJetWallet.Sdk.Service;
using Newtonsoft.Json;
using Service.Circle.Signer;
using Service.Unlimint.Signer.Grpc;
using Service.Unlimint.Signer.Grpc.Models;

// ReSharper disable InconsistentLogPropertyNaming

namespace Service.Unlimint.Signer.Services
{
    public class CircleDepositAddressService : ICircleDepositAddressService
    {
        private readonly ICircleAssetMapper _assetMapper;
        private readonly ICircleBlockchainMapper _blockchainMapper;
        private readonly IApiKeyStorage _apiKeyStorage;
        private readonly CircleClient _circleClient;
        private readonly ILogger<CircleDepositAddressService> _logger;

        public CircleDepositAddressService(
            ILogger<CircleDepositAddressService> logger,
            ICircleAssetMapper assetMapper,
            ICircleBlockchainMapper blockchainMapper,
            IApiKeyStorage apiKeyStorage)
        {
            _logger = logger;
            _assetMapper = assetMapper;
            _blockchainMapper = blockchainMapper;
            _apiKeyStorage = apiKeyStorage;

            _circleClient = new CircleClient(null, Program.Settings.CircleNetwork);
        }

        public async Task<Response<CreateCircleDepositAddressResponse>> GenerateDepositAddress(
            CreateCircleDepositAddressRequest request)
        {
            _logger.LogInformation("GenerateDepositAddress request: {request}", JsonConvert.SerializeObject(request));

            using var action = MyTelemetry.StartActivity("Generate Circle Deposit Address");
            request.AddToActivityAsJsonTag("request");

            try
            {
                var circleAsset = _assetMapper.AssetToCircleAsset(request.BrokerId, request.Asset);
                if (circleAsset == null)
                {
                    _logger.LogError("Circle asset is not configured for asset {asset}", request.Asset);
                    return Response<CreateCircleDepositAddressResponse>.Error("Circle asset is not configured");
                }

                var circleBlockchain =
                    _blockchainMapper.BlockchainToCircleBlockchain(request.BrokerId, request.Blockchain);
                if (circleBlockchain == null)
                {
                    _logger.LogError("Circle blockchain is not configured for blockchain {blockchain}",
                        request.Blockchain);
                    return Response<CreateCircleDepositAddressResponse>.Error("Circle blockchain is not configured");
                }

                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured for broker {brokerId}",
                        request.BrokerId);
                    return Response<CreateCircleDepositAddressResponse>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);
                var response = await _circleClient.GenerateAddressAsync(circleAsset.CircleWalletId, request.RequestGuid,
                    circleAsset.CircleAsset, circleBlockchain.CircleBlockchain);

                if (!response.Success) return Response<CreateCircleDepositAddressResponse>.Error(response.Message);

                if (!string.IsNullOrEmpty(response.Data.AddressTag) &&
                    string.IsNullOrEmpty(circleBlockchain.TagSeparator))
                {
                    _logger.LogError("Tag separator is not configured for blockchain {blockchain}",
                        circleBlockchain.CircleBlockchain);
                    return Response<CreateCircleDepositAddressResponse>.Error("Tag separator is not configured");
                }

                var address = string.IsNullOrEmpty(response.Data.AddressTag)
                    ? response.Data.Address
                    : $"{response.Data.Address}{circleBlockchain.TagSeparator}{response.Data.AddressTag}";

                address.AddToActivityAsTag("address");

                return Response<CreateCircleDepositAddressResponse>.Success(
                    new CreateCircleDepositAddressResponse
                    {
                        Address = address
                    });
            }
            catch (Exception exception)
            {
                exception.FailActivity();
                return Response<CreateCircleDepositAddressResponse>.Error(exception.Message);
            }
        }
    }
}