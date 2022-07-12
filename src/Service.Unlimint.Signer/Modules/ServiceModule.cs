﻿using Autofac;
using MyJetWallet.ApiSecurityManager.Autofac;
using MyJetWallet.Circle;
using MyJetWallet.Sdk.NoSql;
using Service.PersonalData.Client;
using Service.Unlimint.Signer.Services;

namespace Service.Unlimint.Signer.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var myNoSqlClient = builder.CreateNoSqlClient(Program.Settings.MyNoSqlReaderHostPort, 
                Program.LogFactory);

            builder.RegisterPersonalDataClient(Program.Settings.PersonalDataGrpcServiceUrl);
            builder.RegisterEncryptionServiceClient();
            builder.RegisterType<RsaKeyStorage>().AsSelf().SingleInstance();

            CircleClient.PrintPostApiCalls = true;
            CircleClient.PrintPutApiCalls = true;
        }
    }
}