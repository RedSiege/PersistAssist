using PersistAssist.Models;

namespace PersistAssist.Function
{
    public class SvcList : Tradecraft
    {
        public override string TradecraftName => "SvcList";

        public override string TradecraftDesc => "Lists services on a machine";

        public override string TradecraftUsage => "PersistAssist.exe -t SvcList -s <service name>";

        public override string TradecraftTask(ParsedArgs pArgs)
        {
            throw new System.NotImplementedException();
        }
    }
}
