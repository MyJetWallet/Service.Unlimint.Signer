using System.Runtime.Serialization;
using MyJetWallet.Unlimint.Models;
using MyJetWallet.Unlimint.Models.Payments;


namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class GetPaymentInfo
    {
        [DataMember(Order = 1)]
        public string Id { get; set; }

        [DataMember(Order = 2)]
        public string Type { get; set; }

        [DataMember(Order = 3)]
        public string MerchantId { get; set; }

        [DataMember(Order = 4)]
        public string MerchantWalletId { get; set; }

        [DataMember(Order = 7)]
        public string Description { get; set; }

        [DataMember(Order = 8)]
        public PaymentStatus? Status { get; set; }

        [DataMember(Order = 9)]
        public UnlimintAmount Amount { get; set; }
        
        [DataMember(Order = 10)]
        public UnlimintAmount Fee { get; set; }
        
        [DataMember(Order = 11)]
        public CardDescription Card { get; set; }
        
        // [DataMember(Order = 11)]
        // public string CaptureDate { get; set; }

        [DataMember(Order = 13)]
        public string TrackingRef { get; set; }

        [DataMember(Order = 14)]
        public PaymentErrorCode? ErrorCode { get; set; }

        [DataMember(Order = 15)]
        public Metadata Metadata { get; set; }

        // [DataMember(Order = 16)]
        // public RiskEvaluation RiskEvaluation { get; set; }

        [DataMember(Order = 17)]
        public string CreateDate { get; set; }

        // [DataMember(Order = 18)]
        // public string UpdateDate { get; set; }

        // [DataMember(Order = 19)]
        // public string TransactionHash { get; set; }
        //
        // [DataMember(Order = 21)]
        // public RequiredAction RequiredAction { get; set; }
    }
}