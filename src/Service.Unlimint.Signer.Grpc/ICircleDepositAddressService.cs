using System.ServiceModel;
using System.Threading.Tasks;
using Service.Unlimint.Signer.Grpc.Models;

namespace Service.Unlimint.Signer.Grpc
{
    [ServiceContract]
    public interface ICircleDepositAddressService
    {
        [OperationContract]
        Task<Response<CreateCircleDepositAddressResponse>> GenerateDepositAddress(
            CreateCircleDepositAddressRequest request);
    }
}