using System;
using System.Runtime.Serialization;
using MyJetWallet.Circle.Models;
using MyJetWallet.Circle.Models.Cards;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class CircleCardDetails
    {
        [DataMember(Order = 1)] public string Id { get; set; }
        [DataMember(Order = 2)] public CardStatus Status { get; set; }
        [DataMember(Order = 3)] public int ExpMonth { get; set; }
        [DataMember(Order = 4)] public int ExpYear { get; set; }
        [DataMember(Order = 5)] public string Network { get; set; }
        [DataMember(Order = 6)] public string Last4 { get; set; }
        [DataMember(Order = 7)] public CardVerificationError? ErrorCode { get; set; }
        [DataMember(Order = 8)] public DateTime CreateDate { get; set; }
        [DataMember(Order = 9)] public DateTime UpdateDate { get; set; }
        [DataMember(Order = 10)] public string Bin { get; set; }
        [DataMember(Order = 11)]public CardFundingType FundingType { get; set; }
        [DataMember(Order = 12)]public string Fingerprint { get; set; }
        [DataMember(Order = 13)]public RiskEvaluation RiskEvaluation { get; set; }
        [DataMember(Order = 14)] public string IssuerCountry { get; set; }
    }
}