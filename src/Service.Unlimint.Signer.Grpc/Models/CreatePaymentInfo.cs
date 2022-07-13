using System.Runtime.Serialization;
using MyJetWallet.Unlimint.Models.Payments;


namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class CreatePaymentInfo
    {
        [DataMember(Order = 1)] public string Id { get; set; }
        [DataMember(Order = 2)] public string RedirectUrl { get; set; }
        // [DataMember(Order = 3)] public PaymentStatus Status { get; set; }
        // [DataMember(Order = 4)] public string TrackingRef { get; set; }
        // [DataMember(Order = 5)] public PaymentErrorCode? ErrorCode { get; set; }

    }
}