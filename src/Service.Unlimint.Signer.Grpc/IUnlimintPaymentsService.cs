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
        Task<Response<CreatePaymentInfo>> CreateUnlimintPaymentAsync(CreatePaymentRequest request);

        [OperationContract]
        Task<Response<GetPaymentInfo>> GetUnlimintPaymentByIdAsync(GetPaymentByIdRequest byIdRequest);
        
        [OperationContract]
        Task<Response<GetPaymentInfo>> GetUnlimintPaymentByMerchantIdAsync(GetPaymentByMerchantIdRequest byIdRequest);
    }
}