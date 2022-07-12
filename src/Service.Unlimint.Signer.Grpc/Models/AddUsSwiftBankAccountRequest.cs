using System.Runtime.Serialization;
using MyJetWallet.Circle.Models.WireTransfers;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class AddUsSwiftBankAccountRequest
    {
        [DataMember(Order = 1)]
        public string IdempotencyKey { get; set; }

        [DataMember(Order = 2)]
        public string AccountNumber { get; set; }

        [DataMember(Order = 3)]
        public string RoutingNumber { get; set; }

        [DataMember(Order = 4)]
        public BillingDetails BillingDetails { get; set; }

        [DataMember(Order = 5)]
        public BankAddress BankAddress { get; set; }

        [DataMember(Order = 6)]
        public string BrokerId { get; set; }
    }
}