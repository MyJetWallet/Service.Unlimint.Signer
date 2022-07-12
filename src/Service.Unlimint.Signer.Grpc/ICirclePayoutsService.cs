using System.ServiceModel;
using System.Threading.Tasks;
using MyJetWallet.Circle.Models.Payouts;
using Service.Unlimint.Signer.Grpc.Models;
using Service.Unlimint.Signer.Grpc.Models.BusinessAccount;

namespace Service.Unlimint.Signer.Grpc
{
    [ServiceContract]
    public interface ICirclePayoutsService
    {
        [OperationContract]
        Task<Response<CirclePayoutInfo>> AddCirclePayout(AddPayoutRequest request);

        [OperationContract]
        Task<Response<PayoutInfo>> GetCirclePayoutInfo(GetPayoutRequest request);

        [OperationContract]
        Task<Response<PayoutInfo[]>> GetCirclePayoutsInfo(CirclePaginationRequest request);
    }
}