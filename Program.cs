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
                    {"cmd|command=", "Command to use as payload", opt => pArgs.Command = opt },
                    {"rk|rootkey=", "Root key for registry operations", opt => pArgs.regRootKey = opt },
                    {"sk|subkey=", "Sub key for registry operations", opt => pArgs.regSubKey = opt },
                    {"kv|keyvalue=", "Value to assign regirsty key", opt => pArgs.regKeyValue = opt},
                    {"rc|registrycontext=", "Context to write reg key to (options: hkcu or hklm)", opt => pArgs.regContext = opt },
                    {"tn|taskname=", "Task name to set for MSBuild operations", opt => pArgs.TaskName = opt},
                    {"pl|payload=", "Payload to substitute into template", opt => pArgs.Payload = opt},
                    {"fp|filepath=", "Path to file/directory to target", opt => pArgs.filePath = opt},
                    {"dp|duplicatepath=", "Path to duplicate file times from, modified all timestamps", opt =>pArgs.dupPath = opt },
                    {"ts|timestamp=", "Specify M(odified), A(ccessed), or C(reated) timestamp. Use ALL to target all timestamps", opt => pArgs.Timestamp = opt},
                    {"nt|newtime=", "Specify a new date to change specified timestamp to", opt => pArgs.newTime = opt },
                    {"un|username=", "Specify username for credCheck", opt => pArgs.userName = opt },
                    {"pw|passwd=", "Specify password for credCheck", opt => pArgs.Passwd = opt },
                    {"efq|eventFilterQuery=", "EventFilter query for WMI event subscription", opt => pArgs.eventFilterQuery = opt},
                    {"efn|eventFilterName=", "EventFilter name for WMI event subscription", opt => pArgs.eventFilterName = opt },
                    {"ecn|eventConsumerName=", "EventConsumer name for WMI event subscription", opt => pArgs.eventConsumerName = opt},
                    {"ecv|eventConsumerValue=", "EventConsumer value for WMI event subscription", opt => pArgs.eventConsumerValue = opt },
                    {"q|query=", "Query to run", opt => pArgs.Query = opt },
                    {"dn|domain=", "Specify current domain", opt => pArgs.domain = opt },
                    {"p|persist", "Execute specified techique", opt => pArgs.Persist = opt != null},
                    {"c|cleanup", "Clean up specified technique", opt => pArgs.Cleanup = opt != null },
                    {"l|list", "List available techniques", opt => pArgs.listModules = opt != null },
                    {"lm|listmodule=", "List available techniques from specified module category", opt => pArgs.listModule = opt },
                    {"i|info", "Displays information on a specified technique", opt => pArgs.moduleInfo = opt != null },
                    {"h|help", "show this message and exit", opt => pArgs.Help = opt != null}                };

                opts.Parse(args);

                UI.Action(pArgs, opts);
            } catch (System.Exception e) { System.Console.WriteLine($"Error: {e.Message}"); }
        }
    }
}
