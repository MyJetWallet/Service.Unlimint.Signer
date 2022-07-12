using Autofac;
using Service.Unlimint.Signer.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.Unlimint.Signer.Client
{
    public static class AutofacHelper
    {
        public static void RegisterCirclePaymentsClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new UnlimintPaymentsClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetCirclePaymentsService()).As<IUnlimintPaymentsService>()
                .SingleInstance();
        }
    }
}