using System.ServiceModel;
using System.Threading.Tasks;
using Service.Unlimint.Signer.Grpc.Models;

namespace Service.Unlimint.Signer.Grpc
{
    [ServiceContract]
    public interface ICircleBankAccountsService
    {
        [OperationContract]
        Task<Response<CircleBankAccountDetails>> AddCircleSepaBankAccount(AddSepaBankAccountRequest bankAccount);

        [OperationContract]
        Task<Response<CircleBankAccountDetails>> AddCircleUsSwiftBankAccount(AddUsSwiftBankAccountRequest bankAccount);

        [OperationContract]
        Task<Response<CircleBankWireTransferDetails>> GetBankWireTransferDetails(GetBankWireTransferDetailsRequest request);
    }
}