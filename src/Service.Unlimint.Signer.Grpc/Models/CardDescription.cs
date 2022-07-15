using System.Runtime.Serialization;
using MyJetWallet.Unlimint.Models.Payments;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class CardDescription
    {
        [DataMember(Order = 1)] public AcctTypeEnum? AcctType { get; set; }

        [DataMember(Order = 2)] public string Expiration { get; set; }

        [DataMember(Order = 3)] public string Holder { get; set; }

        [DataMember(Order = 4)] public string IssuingCountryCode { get; set; }

        [DataMember(Order = 5)] public string MaskedPan { get; set; }

        [DataMember(Order = 6)] //https://documenter.getpostman.com/view/10451813/SzKSTzVu?version=latest#abf9f898-580d-4dec-864e-199381d79c2b
        public string Token { get; set; }
    }
}
