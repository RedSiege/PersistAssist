using Mono.Options;

using PersistAssist.Utils;
using PersistAssist.Models;

namespace PersistAssist
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try {

                if (Data.displayBanner) { UI.Banner(); }

                ParsedArgs pArgs = new ParsedArgs();

                OptionSet opts = new OptionSet() {
                    {"t|technique=", "Persistence technique to use", opt => pArgs.Technique = opt },
                    {"a|action=", "Action to perform", opt => pArgs.Action = opt },
                    {"s|search=", "Keyword to search for", opt => pArgs.Search = opt },
                    {"rk|rootkey=", "Root key for registry operations", opt => pArgs.regRootKey = opt },
                    {"sk|subkey=", "Sub key for registry operations", opt => pArgs.regSubKey = opt },
                    {"kv|keyvalue=", "Value to assign regirsty key", opt => pArgs.regKeyValue = opt},
                    {"tn|taskname=", "Task name to set for MSBuild operations", opt => pArgs.TaskName = opt},
                    {"pl|payload=", "Payload to substitute into template", opt => pArgs.Payload = opt},
                    {"fp|filepath=", "Path to file/directory to target", opt => pArgs.filePath = opt},
                    {"dp|duplicatepath=", "Path to duplicate file times from, modified all timestamps", opt =>pArgs.dupPath = opt },
                    {"ts|timestamp=", "Specify M(odified), A(ccessed), or C(reated) timestamp. Use ALL to target all timestamps", opt => pArgs.Timestamp = opt},
                    {"nt|newtime=", "Specify a new date to change specified timestamp to", opt => pArgs.newTime = opt },
                    {"p|persist", "Execute specified techique", opt => pArgs.Persist = opt != null},
                    {"c|cleanup", "Clean up specified technique", opt => pArgs.Cleanup = opt != null },
                    {"l|list", "List available techniques", opt => pArgs.listModules = opt != null },
                    {"i|info", "Displays information on a specified technique", opt => pArgs.moduleInfo = opt != null },
                    {"h|help", "show this message and exit", opt => pArgs.Help = opt != null}
                };

                opts.Parse(args);

                UI.Action(pArgs, opts);
            } catch (System.Exception e) { System.Console.WriteLine($"Error: {e.Message}"); }
        }
    }
}
