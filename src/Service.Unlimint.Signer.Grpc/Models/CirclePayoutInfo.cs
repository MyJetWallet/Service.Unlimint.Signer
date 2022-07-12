﻿using System.Runtime.Serialization;
using MyJetWallet.Circle.Models.Payouts;

namespace Service.Unlimint.Signer.Grpc.Models
{
    [DataContract]
    public class CirclePayoutInfo
    {
        [DataMember(Order = 1)] public string Id { get; set; }
        [DataMember(Order = 2)] public PayoutStatus Status { get; set; }
        [DataMember(Order = 3)] public string TrackingRef { get; set; }
        [DataMember(Order = 4)] public PayoutErrorCode? ErrorCode { get; set; }
    }
}