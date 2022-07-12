using System;
using System.Runtime.Serialization;
using MyJetWallet.Circle.Models.WireTransfers;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class CircleBankAccountDetails
    {
        [DataMember(Order = 1)]
        public string Id { get; set; }

        [DataMember(Order = 2)]
        public BankAccountStatus Status { get; set; }

        [DataMember(Order = 3)]
        public string Description { get; set; }

        [DataMember(Order = 4)]
        public string TrackingRef { get; set; }

        [DataMember(Order = 5)]
        public string Fingerprint { get; set; }

        [DataMember(Order = 6)]
        public BillingDetails BillingDetails { get; set; }

        [DataMember(Order = 7)]
        public BankAddress BankAddress { get; set; }

        [DataMember(Order = 8)]
        public DateTime CreateDate { get; set; }

        [DataMember(Order = 9)]
        public DateTime UpdateDate { get; set; }
    }
}