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
        
        public static void RegisterCircleApiKeysClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new CircleApiKeysClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetCircleApiKeyService()).As<ICircleApiKeyService>()
                .SingleInstance();
        }

        public static void RegisterCircleEncryptionKeysClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new CircleEncryptionKeysClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetCircleEncryptionKeyService()).As<ICircleEncryptionKeyService>()
                .SingleInstance();
        }

        public static void RegisterCircleCardsClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new CircleCardsClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetCircleCardsService()).As<ICircleCardsService>()
                .SingleInstance();
        }

        public static void RegisterCircleBankAccountsClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new CircleBankAccountsClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetCircleBankAccountsService()).As<ICircleBankAccountsService>()
                .SingleInstance();
        }

        public static void RegisterCirclePaymentsClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new CirclePaymentsClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetCirclePaymentsService()).As<IUnlimintPaymentsService>()
                .SingleInstance();
        }

        public static void RegisterCircleDepositAddressClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new CircleDepositAddressClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetCircleDepositAddressService()).As<ICircleDepositAddressService>()
                .SingleInstance();
        }

        public static void RegisterCircleTransfersClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new CircleTransfersClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetCircleTransfersService()).As<ICircleTransfersService>()
                .SingleInstance();
        }

        public static void RegisterCircleBusinessAccountClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new CircleBusinessAccountClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetCircleBusinessAccountService()).As<ICircleBusinessAccountService>()
                .SingleInstance();
        }

        public static void RegisterCircleChargebackClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new CircleChargebacksClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetCircleChargebacksService()).As<ICircleChargebacksService>()
                .SingleInstance();
        }

        public static void RegisterCirclePayoutClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new CirclePayoutsClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetCirclePayoutsService()).As<ICirclePayoutsService>()
                .SingleInstance();
        }

    }
}