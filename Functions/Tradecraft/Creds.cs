using PersistAssist.Models;

namespace PersistAssist.Functions
{
    internal class Creds : Tradecraft
    {
        public override string TradecraftName => "Creds";

        public override string TradecraftDesc => "Cred operations";

        public override string TradecraftUsage => "PersistAssist -t Creds ";

        public override string TradecraftTask(ParsedArgs pArgs)
        {
            try {
                throw new System.NotImplementedException();
            } catch (System.Exception e) { return $"PersistAssist module failed: {e.Message}"; }
        }
    }
}
