using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;
using Service.Unlimint.Signer.Grpc;

namespace Service.Unlimint.Signer.Client
{
    [UsedImplicitly]
    public class UnlimintSignerClientFactory: MyGrpcClientFactory
    {
        public UnlimintSignerClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
        {
        }

        public IHelloService GetHelloService() => CreateGrpcService<IHelloService>();
    }
}
