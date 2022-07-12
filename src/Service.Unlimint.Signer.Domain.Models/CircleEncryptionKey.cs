using System;
using System.Runtime.Serialization;

namespace Service.Unlimint.Signer.Domain.Models
{
    [DataContract]
    public class CircleEncryptionKey
    {
        [DataMember(Order = 1)] public string BrokerId { get; set; }
        [DataMember(Order = 2)] public string Id { get; set; }
        [DataMember(Order = 3)] public string EncryptionKey { get; set; }
        [DataMember(Order = 4)] public DateTime UpdateDate { get; set; }
    }
}