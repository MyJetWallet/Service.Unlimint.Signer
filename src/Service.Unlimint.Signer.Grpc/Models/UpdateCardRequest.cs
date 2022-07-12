using System.Runtime.Serialization;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class UpdateCardRequest
    {
        [DataMember(Order = 1)] public string BrokerId { get; set; }
        [DataMember(Order = 2)] public string CardId { get; set; }
        [DataMember(Order = 3)] public string KeyId { get; set; }
        [DataMember(Order = 4)] public string EncryptedData { get; set; }
        [DataMember(Order = 5)] public int ExpMonth { get; set; }
        [DataMember(Order = 6)] public int ExpYear { get; set; }
    }
}