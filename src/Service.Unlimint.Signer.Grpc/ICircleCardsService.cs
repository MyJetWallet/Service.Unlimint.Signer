using System.ServiceModel;
using System.Threading.Tasks;
using Service.Unlimint.Signer.Grpc.Models;

namespace Service.Unlimint.Signer.Grpc
{
    [ServiceContract]
    public interface ICircleCardsService
    {
        [OperationContract]
        Task<Response<CircleCardDetails>> GetCircleCard(GetCardRequest request);

        [OperationContract]
        Task<Response<CircleCardDetails>> AddCircleCard(AddCardRequest card);
    }
}