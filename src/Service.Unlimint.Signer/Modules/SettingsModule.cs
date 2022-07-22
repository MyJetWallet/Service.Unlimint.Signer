using Autofac;
using MyJetWallet.Unlimint;

namespace Service.Unlimint.Signer.Modules
{
    public class SettingsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(Program.Settings).AsSelf().SingleInstance();

            if (Program.Settings.PrintPostApiCalls)
                UnlimintClient.PrintPostApiCalls = true;
        }
    }
}