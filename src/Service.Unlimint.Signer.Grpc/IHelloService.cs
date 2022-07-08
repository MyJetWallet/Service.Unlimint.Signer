using System.ServiceModel;
using System.Threading.Tasks;
using Service.Unlimint.Signer.Grpc.Models;

namespace Service.Unlimint.Signer.Grpc
{
    [ServiceContract]
    public interface IHelloService
    {
        [OperationContract]
        Task<HelloMessage> SayHelloAsync(HelloRequest request);
    }
}