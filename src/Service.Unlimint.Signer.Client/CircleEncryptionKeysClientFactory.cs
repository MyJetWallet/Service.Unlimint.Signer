using System;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;
using MyJetWallet.Sdk.GrpcMetrics;
using ProtoBuf.Grpc.Client;
using Service.Unlimint.Signer.Grpc;

namespace Service.Unlimint.Signer.Client
{
    [UsedImplicitly]
    public class CircleEncryptionKeysClientFactory : MyGrpcClientFactory
    {
        private readonly CallInvoker _channel;

        public CircleEncryptionKeysClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var channel = GrpcChannel.ForAddress(grpcServiceUrl);
            _channel = channel.Intercept(new PrometheusMetricsInterceptor());
        }

        public ICircleEncryptionKeyService GetCircleEncryptionKeyService() =>
            _channel.CreateGrpcService<ICircleEncryptionKeyService>();
    }
}