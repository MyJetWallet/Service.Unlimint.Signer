using MyJetWallet.Sdk.Service;
using MyJetWallet.Unlimint;
using MyYamlParser;

namespace Service.Unlimint.Signer.Settings
{
    public class SettingsModel
    {
        [YamlProperty("UnlimintSigner.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("UnlimintSigner.ZipkinUrl")]
        public string ZipkinUrl { get; set; }

        [YamlProperty("UnlimintSigner.ElkLogs")] public LogElkSettings ElkLogs { get; set; }

        [YamlProperty("UnlimintSigner.MyNoSqlReaderHostPort")]
        public string MyNoSqlReaderHostPort { get; set; }

        [YamlProperty("UnlimintSigner.PersonalDataGrpcServiceUrl")]
        public string PersonalDataGrpcServiceUrl { get; set; }

        [YamlProperty("UnlimintSigner.Success3dUrl")]
        public string Success3dUrl { get; set; }

        [YamlProperty("UnlimintSigner.Failure3dUrl")]
        public string Failure3dUrl { get; set; }

        [YamlProperty("UnlimintSigner.UnlimintNetwork")]
        public UnlimintNetwork UnlimintNetwork { get; set; }
        
        [YamlProperty("UnlimintSigner.UnlimintTerminalCode")]
        public string UnlimintTerminalCode { get; set; }
        
        [YamlProperty("UnlimintSigner.UnlimintPassword")]
        public string UnlimintPassword { get; set; }
    }
}