using System.Runtime.Serialization;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class AddPaymentRequest
    {
        [DataMember(Order = 1)] public string BrokerId { get; set; }
        [DataMember(Order = 2)] public string ClientId { get; set; }
        [DataMember(Order = 3)] public string IdempotencyKey { get; set; }
        [DataMember(Order = 4)] public string KeyId { get; set; }
        [DataMember(Order = 5)] public string SessionId { get; set; }
        [DataMember(Order = 6)] public string IpAddress { get; set; }
        [DataMember(Order = 7)] public double Amount { get; set; }
        [DataMember(Order = 8)] public string Currency { get; set; }
        [DataMember(Order = 9)] public string Verification { get; set; }
        [DataMember(Order = 10)] public string SourceId { get; set; }
        [DataMember(Order = 11)] public string SourceType { get; set; }
        [DataMember(Order = 12)] public string Description { get; set; }
        [DataMember(Order = 13)] public string EncryptedData { get; set; }
    }

    [DataContract]
    public class AddTransferV2Request
    {
        [DataMember(Order = 1)] public string BrokerId { get; set; }
        [DataMember(Order = 2)] public string ClientId { get; set; }
        [DataMember(Order = 3)] public string IdempotencyKey { get; set; }
        [DataMember(Order = 4)] public double Amount { get; set; }
        [DataMember(Order = 5)] public string Currency { get; set; }
        [DataMember(Order = 6)] public string SourceId { get; set; }
        [DataMember(Order = 7)] public string SourceType { get; set; }
    }
}