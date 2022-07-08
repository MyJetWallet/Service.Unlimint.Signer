using System.Runtime.Serialization;
using Service.Unlimint.Signer.Domain.Models;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class HelloMessage : IHelloMessage
    {
        [DataMember(Order = 1)]
        public string Message { get; set; }
    }
}