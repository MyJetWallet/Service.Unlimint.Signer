using System.ServiceModel;
using System.Threading.Tasks;
using Service.Unlimint.Signer.Domain.Models;
using Service.Unlimint.Signer.Grpc.Models;

namespace Service.Unlimint.Signer.Grpc
{
    [ServiceContract]
    public interface ICircleEncryptionKeyService
    {
        [OperationContract]
        Task<Response<CircleEncryptionKey>> GetCircleEncryptionKey(GetCircleEncryptionKeyRequest request);

        [OperationContract]
        Task<Response<string>> CircleEncryptionData(CircleEncryptDataRequest request);
    }
}