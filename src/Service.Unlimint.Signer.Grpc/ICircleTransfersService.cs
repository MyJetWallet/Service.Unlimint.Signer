using System.Threading.Tasks;
using MyJetWallet.Circle.Models.Transfers;
using Service.Unlimint.Signer.Grpc.Models;

namespace Service.Unlimint.Signer.Grpc
{
    public interface ICircleTransfersService
    {
        Task<Response<TransferInfo>> AddCircleTransfer(AddTransferRequest request);
        Task<Response<TransferInfo>> GetCircleTransferInfo(GetTransfersV2Request request);
        Task<Response<TransferInfo[]>> GetCircleTransfersInfo(GetTransfersRequest request);
    }
}