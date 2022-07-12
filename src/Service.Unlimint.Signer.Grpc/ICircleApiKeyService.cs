using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using Service.Unlimint.Signer.Domain.Models;
using Service.Unlimint.Signer.Grpc.Models;

namespace Service.Unlimint.Signer.Grpc
{
    [ServiceContract]
    public interface ICircleApiKeyService
    {
        [OperationContract]
        Task<Response<List<CircleApiKey>>> GetCircleApiKeysList();

        [OperationContract]
        Task<Response<CircleApiKey>> GetCircleApiKey(GetCircleApiKeyRequest request);

        [OperationContract]
        Task<Response<bool>> AddCircleApiKey(CircleApiKey apiKey);

        [OperationContract]
        Task<Response<bool>> UpdateCircleApiKey(CircleApiKey user);

        [OperationContract]
        Task<Response<bool>> RemoveCircleApiKey(RemoveCircleApiKeyRequest request);
    }
}