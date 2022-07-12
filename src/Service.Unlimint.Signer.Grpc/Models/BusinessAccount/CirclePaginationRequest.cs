using System.Runtime.Serialization;

namespace Service.Unlimint.Signer.Grpc.Models.BusinessAccount
{
    [DataContract]
    public class CirclePaginationRequest
    {
        [DataMember(Order = 1)] public string TakeAfter { get; set; }
        [DataMember(Order = 2)] public int PageSize { get; set; }
    }
    
}