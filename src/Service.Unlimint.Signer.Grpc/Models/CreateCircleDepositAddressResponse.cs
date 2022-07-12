using System.Runtime.Serialization;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class CreateCircleDepositAddressResponse
    {
        [DataMember(Order = 1)] public string Address { get; set; }
    }
}