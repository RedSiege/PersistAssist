using PersistAssist.Models;

namespace PersistAssist.Function
{ 
    internal class SvcList : Tradecraft
    {
        public override string TradecraftName => "SvcList";

        public override string TradecraftDesc => "Lists services on a machine";

        public override string TradecraftUsage => "PersistAssist.exe -t SvcList -s <service name>";

        public override string TradecraftTask(ParsedArgs pArgs)
        {
            try {
                throw new System.NotImplementedException();
            } catch (System.Exception e) { return $"PersistAssist module failed: {e.Message}"; } 
        }
    }
}
