using System.Runtime.Serialization;
using MyJetWallet.Unlimint.Models.Payments;


namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class UnlimintPaymentInfo
    {
        [DataMember(Order = 1)] public string Id { get; set; }
        [DataMember(Order = 2)] public PaymentStatus Status { get; set; }
        [DataMember(Order = 3)] public string TrackingRef { get; set; }
        [DataMember(Order = 4)] public PaymentErrorCode? ErrorCode { get; set; }
        [DataMember(Order = 5)] public string RedirectUrl { get; set; }
    }
}