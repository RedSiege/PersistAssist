using PersistAssist.Models;

namespace PersistAssist.Functions
{
    public class SchList : Tradecraft
    {
        public override string TradecraftName => "SchList";

        public override string TradecraftDesc => "Lists scheduled tasks on a machine";

        public override string TradecraftUsage => "PersistAssist.exe -t SchList -s <scheduled task name>";

        public override string TradecraftTask(ParsedArgs pArgs)
        {
            try {
                throw new System.NotImplementedException();
            } catch (System.Exception e) { return $"PersistAssist module failed: {e.Message}"; }
        }
    }
}
