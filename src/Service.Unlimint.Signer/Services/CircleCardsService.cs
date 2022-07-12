using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.ApiSecurityManager.ApiKeys;
using MyJetWallet.Circle;
using MyJetWallet.Circle.Models;
using MyJetWallet.Circle.Models.Cards;
using MyJetWallet.Sdk.Service;
using Service.Circle.Signer;
using Service.PersonalData.Grpc;
using Service.PersonalData.Grpc.Contracts;
using Service.Unlimint.Signer.Grpc;
using Service.Unlimint.Signer.Grpc.Models;

// ReSharper disable InconsistentLogPropertyNaming

namespace Service.Unlimint.Signer.Services
{
    public class CircleCardsService : ICircleCardsService
    {
        private readonly CircleClient _circleClient;
        private readonly ILogger<CircleCardsService> _logger;
        private readonly IPersonalDataServiceGrpc _personalDataServiceGrpc;
        private readonly IApiKeyStorage _apiKeyStorage;

        public CircleCardsService(ILogger<CircleCardsService> logger,
            IPersonalDataServiceGrpc personalDataServiceGrpc,
            IApiKeyStorage apiKeyStorage
            )
        {
            _logger = logger;
            
            _personalDataServiceGrpc = personalDataServiceGrpc;
            _apiKeyStorage = apiKeyStorage;
            _circleClient = new CircleClient(null, Program.Settings.CircleNetwork);
        }

        public async Task<Response<CircleCardDetails>> GetCircleCard(GetCardRequest request)
        {
            using var action = MyTelemetry.StartActivity("Get Circle card");
            request.AddToActivityAsJsonTag("request");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured for broker {brokerId}",
                        request.BrokerId);
                    return Response<CircleCardDetails>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var response = await _circleClient.GetCardAsync(request.CardId);

                if (!response.Success) return Response<CircleCardDetails>.Error(response.Message);

                response.Data.Last4.AddToActivityAsTag("last4");
                return Response<CircleCardDetails>.Success(
                    new CircleCardDetails
                    {
                        Id = response.Data.Id,
                        Status = response.Data.Status,
                        ExpMonth = response.Data.ExpMonth,
                        ExpYear = response.Data.ExpYear,
                        Network = response.Data.Network,
                        Last4 = response.Data.Last4,
                        ErrorCode = response.Data.ErrorCode,
                        CreateDate = response.Data.CreateDate,
                        UpdateDate = response.Data.UpdateDate,
                        Bin = response.Data.Bin,
                        FundingType = response.Data.FundingType,
                        Fingerprint = response.Data.Fingerprint,
                        RiskEvaluation = response.Data.RiskEvaluation,
                        IssuerCountry = response.Data.IssuerCountry,
                    });
            }
            catch (Exception exception)
            {
                return Response<CircleCardDetails>.Error(exception.Message);
            }
        }

        public async Task<Response<CircleCardDetails>> AddCircleCard(AddCardRequest card)
        {
            using var action = MyTelemetry.StartActivity("Add Circle card");
            try
            {
                var apiKey = _apiKeyStorage.Get(Program.Settings.ApiKeyId);
                if (string.IsNullOrEmpty(apiKey?.ApiKeyValue))
                {
                    _logger.LogError("Api key is not configured for broker {brokerId}",
                        card.BrokerId);
                    return Response<CircleCardDetails>.Error("Api key is not configured");
                }

                _circleClient.SetAccessToken(apiKey.ApiKeyValue);

                var personalData = await _personalDataServiceGrpc.GetByIdAsync(new GetByIdRequest
                {
                    Id = card.ClientId
                });

                if (personalData?.PersonalData == null)
                {
                    _logger.LogError("Unable to get personal data for client {clientId}",
                        card.ClientId);
                    return Response<CircleCardDetails>.Error("Unable to get personal data for client");
                }



                WebCallResult<CardInfo> response;
                
                if (string.IsNullOrEmpty(""))
                {
                    response = await _circleClient.CreateCardAsync(card.IdempotencyKey, card.KeyId, card.EncryptedData,
                        card.BillingName, card.BillingCity, card.BillingCountry, card.BillingLine1, card.BillingLine2,
                        card.BillingDistrict, card.BillingPostalCode, card.ExpMonth, card.ExpYear,
                        personalData.PersonalData.Email,
                        null, card.SessionId, card.IpAddress);
                }
                else
                {
                    response = await _circleClient.CreateCardAsync(card.IdempotencyKey, card.KeyId, card.EncryptedData,
                        card.BillingName, card.BillingCity, card.BillingCountry, card.BillingLine1, card.BillingLine2,
                        card.BillingDistrict, card.BillingPostalCode, card.ExpMonth, card.ExpYear,
                        personalData.PersonalData.Email,
                        personalData.PersonalData.Phone, card.SessionId, card.IpAddress);
                }

                var req = new {
                    card.IdempotencyKey,
                    card.KeyId,
                    card.EncryptedData,
                    card.BillingName,
                    card.BillingCity,
                    card.BillingCountry,
                    card.BillingLine1,
                    card.BillingLine2,
                    card.BillingDistrict,
                    card.BillingPostalCode,
                    card.ExpMonth,
                    card.ExpYear,
                    personalData.PersonalData.Email,
                    personalData.PersonalData.Phone,
                    card.SessionId,
                    card.IpAddress
                };

                if (!response.Success) {
                    _logger.LogError("CIRCLE CARD ADD ERROR request: {req} - response: {response}",
                         req.ToJson()
                        ,response.ToJson());

                    return Response<CircleCardDetails>.Error(response.Message);
                }

                response.Data.Last4.AddToActivityAsTag("last4");
                return Response<CircleCardDetails>.Success(
                    new CircleCardDetails
                    {
                        Id = response.Data.Id,
                        Status = response.Data.Status,
                        ExpMonth = response.Data.ExpMonth,
                        ExpYear = response.Data.ExpYear,
                        Network = response.Data.Network,
                        Last4 = response.Data.Last4,
                        ErrorCode = response.Data.ErrorCode,
                        CreateDate = response.Data.CreateDate,
                        UpdateDate = response.Data.UpdateDate
                    });
            }
            catch (Exception ex)
            {
                ex.FailActivity();
                return Response<CircleCardDetails>.Error(ex.Message);
            }
        }
    }
}