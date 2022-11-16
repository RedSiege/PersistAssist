using System;
using System.Linq;
using System.Reflection;

using Mono.Options;

using PersistAssist.Models;

using static System.Console;

using static PersistAssist.Models.Data;

namespace PersistAssist.Utils
{
    public class UI
    {
        public static void Banner() { WriteLine(banner); }

        public static void Action(ParsedArgs pArgs, OptionSet opts)
        {
            try
            {

                FunctionInit();

                if (pArgs.Help) { helpMenu(opts); return; }

                if (pArgs.listModules) { listModules(); return; }

                if (!pArgs.listModule.isEmpty()) { listModules(pArgs.listModule); return; }

                Persist persist = Lists._persist.FirstOrDefault(cPersist => cPersist.PersistName.Equals(pArgs.Technique, StringComparison.InvariantCultureIgnoreCase));
                Tradecraft tradecraft = Lists._tradecraft.FirstOrDefault(cTradecraft => cTradecraft.TradecraftName.Equals(pArgs.Technique, StringComparison.InvariantCultureIgnoreCase));
                if (persist is null && tradecraft is null) { throw new PersistAssistException($"Module {pArgs.Technique} doesn't exist\n"); }

                if (pArgs.moduleInfo)
                {
                    if (persist != null) { moduleDetails(persist); return; }
                    else { moduleDetails(tradecraft); return; }
                }

                if (persist != null)
                {
                    if (pArgs.Persist == false && pArgs.Cleanup == false) { throw new PersistAssistException("[-] Specify persistence or cleanup"); }

                    if (persist.requiresAdmin is true && !Extensions.isAdmin())
                    {
                        throw new PersistAssistException("[-] Must be running as Admin to perform this technique");
                    }

                    if (pArgs.Persist) { WriteLine(persist.PersistExec(pArgs)); return; }
                    if (pArgs.Cleanup) { WriteLine(persist.PersistCleanup(pArgs)); return; }
                }
                else
                {
                    WriteLine(tradecraft.TradecraftTask(pArgs)); return;
                }
            }
            catch (PersistAssistException e) { WriteLine(e.Message); }
            catch (Exception e) { WriteLine(e); }
        }

        public static void listModules(string module)
        {

            if (module.ToLower() == "persistence")
            {
                WriteLine($"{module.toTitle()}:\n{string.Concat(Enumerable.Repeat("=", module.Length))}");
                foreach (Enums.PersistType persistType in Enum.GetValues(typeof(Enums.PersistType)))
                {
                    WriteLine($"{persistType}: ");
                    foreach (Persist persist in Lists._persist)
                    {
                        if (persist.PersistCategory == persistType) { WriteLine($"\t{persist.PersistName} - {persist.PersistDesc}"); }
                    }
                }
            }
            else if (module.ToLower() == "tradecraft")
            {
                WriteLine($"{module.toTitle()}:\n{string.Concat(Enumerable.Repeat("=", module.Length))}");
                foreach (Tradecraft tradecraft in Lists._tradecraft) { WriteLine($"\t{tradecraft.TradecraftName} - {tradecraft.TradecraftDesc}"); }
            }
            else if (module.ToLower() == "payloads")
            {
                WriteLine($"{module.toTitle()}:\n{string.Concat(Enumerable.Repeat("=", module.Length))}");
                foreach (Enums.PayloadLang payloadLang in Enum.GetValues(typeof(Enums.PayloadLang)))
                {
                    WriteLine($"{payloadLang}:");
                    foreach (Payload payload in Lists._payloads)
                    {
                        if (payload.PayloadLanguage == payloadLang) { WriteLine($"\t{payload.PayloadName} - {payload.PayloadDesc}"); }
                    }
                }
            }
            else { throw new PersistAssistException("Module category does not exist"); }
        }

        public static void listModules()
        {
            WriteLine("[*] Available modules:\n");

            WriteLine("Persistence:\n============");

            foreach (Enums.PersistType persistType in Enum.GetValues(typeof(Enums.PersistType)))
            {
                WriteLine($"{persistType}: ");
                foreach (Persist persist in Lists._persist)
                {
                    if (persist.PersistCategory == persistType) { WriteLine($"\t{persist.PersistName} - {persist.PersistDesc}"); }
                }
            }

            WriteLine("\nTradecraft:\n===========");
            foreach (Tradecraft tradecraft in Lists._tradecraft) { WriteLine($"\t{tradecraft.TradecraftName} - {tradecraft.TradecraftDesc}"); }

            WriteLine("\nPayloads:\n=========");
            foreach (Enums.PayloadLang payloadLang in Enum.GetValues(typeof(Enums.PayloadLang)))
            {
                WriteLine($"{payloadLang}:");
                foreach (Payload payload in Lists._payloads)
                {
                    if (payload.PayloadLanguage == payloadLang) { WriteLine($"\t{payload.PayloadName} - {payload.PayloadDesc}"); }
                }
            }

        }

        public static void moduleDetails(Persist persist)
        {

            WriteLine(
                $"Name:          {persist.PersistName}\n" +
                $"Desc:          {persist.PersistDesc}\n" +
                $"Usage:         {persist.PersistUsage}\n" +
                $"Category:      {persist.PersistCategory}\n" +
                $"Author:        {persist.Author}\n" +
                $"RequiresAdmin: {persist.requiresAdmin}\n"
                );
        }

        public static void moduleDetails(Tradecraft tradecraft)
        {
            WriteLine(
                $"Name:     {tradecraft.TradecraftName}\n" +
                $"Desc:     {tradecraft.TradecraftDesc}\n" +
                $"Usage:    {tradecraft.TradecraftUsage}\n" +
                $"Author:   {tradecraft.Author}\n"
                );
        }

        public static void helpMenu(OptionSet opts)
        {
            WriteLine(
                "Usage: PersistAssist.exe -t [technique] -<extra options>\n" +
                "Provide the persist technique and what to do with the technique (persist, cleanup, display info)\n" +
                "To list all available persistence techiques, use PersistAssist.exe -l"
                );
            opts.WriteOptionDescriptions(Console.Out);
        }

        public static void PayloadInit()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsSubclassOf(typeof(Payload)))
                {
                    Payload payload = (Payload)Activator.CreateInstance(type);
                    Lists._payloads.Add(payload);
                }
            }
        }

        public static void PersistInit()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsSubclassOf(typeof(Persist)))
                {
                    Persist persist = (Persist)Activator.CreateInstance(type);
                    Lists._persist.Add(persist);
                }
            }
        }

        public static void TradecraftInit()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsSubclassOf(typeof(Tradecraft)))
                {
                    Tradecraft tradecraft = (Tradecraft)Activator.CreateInstance(type);
                    Lists._tradecraft.Add(tradecraft);
                }
            }
        }

        public static void FunctionInit()
        {
            if (Lists._persist.Count == 0) { PersistInit(); }
            if (Lists._tradecraft.Count == 0) { TradecraftInit(); }
            if (Lists._payloads.Count == 0) { PayloadInit(); }
        }
    }
}
