using PersistAssist.Utils;
using PersistAssist.Models;

using static PersistAssist.Utils.SchTaskOps;

namespace PersistAssist.Functions
{
    public class SchList : Tradecraft
    {
        public override string TradecraftName => "SchList";

        public override string TradecraftDesc => "Lists scheduled tasks on a machine";

        public override string TradecraftUsage => "PersistAssist.exe -t SchList -s <scheduled task name> -o <out file>";

        public override string TradecraftTask(ParsedArgs pArgs)
        {
            if (!pArgs.Search.isEmpty())
            {
                return TaskInfo(pArgs.Search);
            }
            else { return ListTasks(); }

        }
    }
}
