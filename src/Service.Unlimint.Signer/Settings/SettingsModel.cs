using MyJetWallet.Circle;
using MyJetWallet.Sdk.Service;
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

        [YamlProperty("UnlimintSigner.MyNoSqlWriterUrl")]
        public string MyNoSqlWriterUrl { get; set; }

        [YamlProperty("UnlimintSigner.MyNoSqlReaderHostPort")]
        public string MyNoSqlReaderHostPort { get; set; }

        [YamlProperty("UnlimintSigner.EncryptionKeysUpdateIntervalSec")]
        public long EncryptionKeysUpdateIntervalSec { get; set; }

        [YamlProperty("UnlimintSigner.PersonalDataGrpcServiceUrl")]
        public string PersonalDataGrpcServiceUrl { get; set; }

        [YamlProperty("UnlimintSigner.ApiKeyId")]
        public string ApiKeyId { get; set; }

        [YamlProperty("UnlimintSigner.Success3dUrl")]
        public string Success3dUrl { get; set; }

        [YamlProperty("UnlimintSigner.Failure3dUrl")]
        public string Failure3dUrl { get; set; }

        [YamlProperty("UnlimintSigner.CircleNetwork")]
        public CircleNetwork CircleNetwork { get; set; }
    }
}