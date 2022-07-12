using System.Runtime.Serialization;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class RemoveCircleEncryptionKeyRequest
    {
        [DataMember(Order = 1)] public string BrokerId { get; set; }
    }
}