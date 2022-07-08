using Autofac;
using Service.Unlimint.Signer.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.Unlimint.Signer.Client
{
    public static class AutofacHelper
    {
        public static void RegisterUnlimintSignerClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new UnlimintSignerClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetHelloService()).As<IHelloService>().SingleInstance();
        }
    }
}
