using System.Runtime.Serialization;
using MyJetWallet.Circle.Models.WireTransfers;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class CircleBankWireTransferDetails
    {
        [DataMember(Order = 1)]
        public string TrackingRef { get; set; }

        [DataMember(Order = 2)]
        public BankWireTransferDetailBeneficiary Beneficiary { get; set; }

        [DataMember(Order = 3)]
        public BankWireTransferDetailBeneficiaryBank BeneficiaryBank { get; set; }
    }
}