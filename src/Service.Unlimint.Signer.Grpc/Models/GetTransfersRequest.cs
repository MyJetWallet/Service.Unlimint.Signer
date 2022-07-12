using System.Runtime.Serialization;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class GetTransfersRequest
    {
        [DataMember(Order = 1)] public string BrokerId { get; set; }
        [DataMember(Order = 2)] public string LastTransferId { get; set; }
        [DataMember(Order = 3)] public int PageSize { get; set; }
    }
}