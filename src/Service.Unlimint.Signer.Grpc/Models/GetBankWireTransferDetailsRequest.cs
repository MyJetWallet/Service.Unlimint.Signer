using System.Runtime.Serialization;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class GetBankWireTransferDetailsRequest
    {
        [DataMember(Order = 1)]
        public string BankAccountId { get; set; }

        [DataMember(Order = 2)]
        public string BrokerId { get; set; }

    }
}