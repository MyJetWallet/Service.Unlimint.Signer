using System.ServiceModel;
using System.Threading.Tasks;
using MyJetWallet.Circle.Models.ChargeBacks;
using Service.Unlimint.Signer.Grpc.Models;
using Service.Unlimint.Signer.Grpc.Models.BusinessAccount;

namespace Service.Unlimint.Signer.Grpc
{
    [ServiceContract]
    public interface ICircleChargebacksService
    {
        [OperationContract]
        Task<Response<Chargeback[]>> GetChargebacks(CirclePaginationRequest request);
    }
}