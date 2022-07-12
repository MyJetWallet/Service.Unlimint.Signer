using Autofac;
using MyJetWallet.ApiSecurityManager.Autofac;
using MyJetWallet.Circle;
using MyJetWallet.Circle.Settings.Ioc;
using MyJetWallet.Sdk.NoSql;
using MyJetWallet.Sdk.Service;
using Service.PersonalData.Client;
using Service.Unlimint.Signer.Jobs;
using Service.Unlimint.Signer.Services;

namespace Service.Unlimint.Signer.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var myNoSqlClient = builder.CreateNoSqlClient(Program.Settings.MyNoSqlReaderHostPort, Program.LogFactory,
                ApplicationEnvironment.HostName ??
                $"{ApplicationEnvironment.AppName}:{ApplicationEnvironment.AppVersion}");

            builder
                .RegisterType<UpdateEncryptionKeysJob>()
                .AsSelf()
                .SingleInstance();

            builder
                .RegisterType<SetSourceWalletIdJob>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterPersonalDataClient(Program.Settings.PersonalDataGrpcServiceUrl);
            builder.RegisterCircleSettingsReader(myNoSqlClient);
            builder.RegisterEncryptionServiceClient();
            builder.RegisterType<RsaKeyStorage>().AsSelf().SingleInstance();

            CircleClient.PrintPostApiCalls = true;
            CircleClient.PrintPutApiCalls = true;
        }
    }
}