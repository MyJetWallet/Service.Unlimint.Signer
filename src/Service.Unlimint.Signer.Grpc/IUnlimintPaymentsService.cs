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
        Task<Response<UnlimintPaymentInfo>> AddUnlimintPayment(AddPaymentRequest request);

        [OperationContract]
        Task<Response<PaymentInfo>> GetUnlimintPaymentInfo(GetPaymentRequest request);
    }
}