using System.ServiceModel;
using System.Threading.Tasks;
using MyJetWallet.Circle.Models.BusinessAccounts;
using Service.Unlimint.Signer.Grpc.Models;
using Service.Unlimint.Signer.Grpc.Models.BusinessAccount;

namespace Service.Unlimint.Signer.Grpc
{
    [ServiceContract]
    public interface ICircleBusinessAccountService
    {
        [OperationContract]
        Task<Response<DepositAddressInfo>> CreateDepositAddress(CreateDepositAddressRequest request);

        [OperationContract]
        Task<Response<RecipientAddressInfo>> CreateRecipientAddress(CreateBusinessRecipientAddressRequest request);

        [OperationContract]
        Task<Response<TransferInfo>> CreateTransfer(CreateBusinessTransferRequest request);

        [OperationContract]
        Task<Response<BalanceInfo>> GetBalances();

        [OperationContract]
        Task<Response<DepositInfo[]>> GetBusinessDeposits(CirclePaginationRequest request);

        [OperationContract]
        Task<Response<RecipientAddressInfo[]>> GetBusinessRecipientAddresses(CirclePaginationRequest request);

        [OperationContract]
        Task<Response<ConfigurationInfo>> GetConfigurationInfo();

        [OperationContract]
        Task<Response<DepositAddressInfo[]>> GetDepositAddresses();

        [OperationContract]
        Task<Response<TransferInfo[]>> GetTransfers(CirclePaginationRequest request);
    }
}