using PersistAssist.Models;

namespace PersistAssist.Functions
{
    public class Creds : Tradecraft
    {
        public override string TradecraftName => "Creds";

        public override string TradecraftDesc => "Cred operations";

        public override string TradecraftUsage => "PersistAssist -t Creds ";

        public override string TradecraftTask(ParsedArgs pArgs)
        {
            throw new System.NotImplementedException();
        }
    }
}
