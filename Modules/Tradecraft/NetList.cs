using PersistAssist.Models;

using static PersistAssist.Utils.Network;

namespace PersistAssist.Functions
{
    class NetList : Tradecraft
    {
        public override string TradecraftName => "NetList";

        public override string TradecraftDesc => "basically ipconfig";

        public override string TradecraftUsage => "PersistAssist.exe -t NetList";

        public override string TradecraftTask(ParsedArgs pArgs)
        {
            return IpConfig();
        }
    }
}
