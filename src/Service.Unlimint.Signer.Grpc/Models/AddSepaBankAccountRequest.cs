using System.Runtime.Serialization;
using MyJetWallet.Circle.Models.WireTransfers;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class AddSepaBankAccountRequest
    {
        [DataMember(Order = 1)]
        public string IdempotencyKey { get; set; }

        [DataMember(Order = 2)]
        public string Iban { get; set; }

        [DataMember(Order = 3)]
        public BillingDetails BillingDetails { get; set; }

        [DataMember(Order = 4)]
        public BankAddress BankAddress { get; set; }

        [DataMember(Order = 5)]
        public string BrokerId { get; set; }
    }
}