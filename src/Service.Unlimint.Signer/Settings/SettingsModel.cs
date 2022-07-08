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

        [YamlProperty("UnlimintSigner.ElkLogs")]
        public LogElkSettings ElkLogs { get; set; }
    }
}
