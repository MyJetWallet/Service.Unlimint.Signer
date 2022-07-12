using System;
using System.Runtime.Serialization;

namespace Service.Unlimint.Signer.Domain.Models
{
    [DataContract]
    public class CircleApiKey
    {
        [DataMember(Order = 1)] public string BrokerId { get; set; }
        [DataMember(Order = 2)] public string ApiKey { get; set; }
        [DataMember(Order = 3)] public DateTime RegisterDate { get; set; }
        [DataMember(Order = 4)] public DateTime UpdateDate { get; set; }
        [DataMember(Order = 5)] public string UpdatedBy { get; set; }
    }
}