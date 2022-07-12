using System.Runtime.Serialization;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class CircleEncryptDataRequest
    {
        [DataMember(Order = 1)] public string Data { get; set; }
        [DataMember(Order = 2)] public string EncryptionKey { get; set; }
    }
}