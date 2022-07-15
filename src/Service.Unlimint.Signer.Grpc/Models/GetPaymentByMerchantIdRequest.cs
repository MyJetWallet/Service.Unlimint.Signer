using System.Runtime.Serialization;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class GetPaymentByMerchantIdRequest
    {
        [DataMember(Order = 1)] public string BrokerId { get; set; }
        [DataMember(Order = 2)] public string MerchantId { get; set; }
    }
}