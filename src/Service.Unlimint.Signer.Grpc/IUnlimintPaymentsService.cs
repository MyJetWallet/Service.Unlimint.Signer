using System.ServiceModel;
using System.Threading.Tasks;
using MyJetWallet.Unlimint.Models.Payments;
using Service.Unlimint.Signer.Grpc.Models;

namespace Service.Unlimint.Signer.Grpc
{
    [ServiceContract]
    public interface IUnlimintPaymentsService
    {
        [OperationContract]
        Task<Response<CreatePaymentInfo>> CreateUnlimintPayment(CreatePaymentRequest request);

        [OperationContract]
        Task<Response<GetPaymentInfo>> GetUnlimintPaymentInfo(GetPaymentRequest request);
    }
}