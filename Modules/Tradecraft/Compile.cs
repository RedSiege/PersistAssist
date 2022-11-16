using System;
using System.Linq;

using PersistAssist.Utils;
using PersistAssist.Models;

using static PersistAssist.Models.Data;
using static PersistAssist.Utils.Compiler;

namespace PersistAssist.Modules
{
    class Compile : Tradecraft
    {
        public override string TradecraftName => "Compile";

        public override string TradecraftDesc => "Standalone utility to compile exes based on C# payloads included in the framework";

        public override string TradecraftUsage => "PeristAssist.exe -t Compile -pl [payload]";

        public override string TradecraftTask(ParsedArgs pArgs)
        {
            if(pArgs.Payload.isEmpty()) { throw new PersistAssistException("Incorrect parameters passed. See technique usage"); }

            Payload payload = Lists._payloads.FirstOrDefault(cPayload => cPayload.PayloadName.Equals(pArgs.Payload, StringComparison.InvariantCultureIgnoreCase));
            if (payload is null) { throw new PersistAssistException($"Module {pArgs.Payload} doesn't exist\n"); }

            return Compile(payload);
        }
    }
}
