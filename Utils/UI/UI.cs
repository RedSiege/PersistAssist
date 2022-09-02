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

        public static void Action(ParsedArgs pArgs, OptionSet opts) {

                FunctionInit();

                if (pArgs.Help) { helpMenu(opts); return; }
                if (pArgs.listModules) { listModules(); return; }

                Persist persist = Lists._persist.FirstOrDefault(cPersist => cPersist.PersistName.Equals(pArgs.Technique, StringComparison.InvariantCultureIgnoreCase));
                Tradecraft tradecraft = Lists._tradecraft.FirstOrDefault(cTradecraft => cTradecraft.TradecraftName.Equals(pArgs.Technique, StringComparison.InvariantCultureIgnoreCase));
                if (persist is null && tradecraft is null) { throw new PersistAssistException($"Module {pArgs.Technique} doesn't exist\n"); }

                if (pArgs.moduleInfo) {
                    if (persist != null) { moduleDetails(persist); return; }
                    else { moduleDetails(tradecraft); return; }
                }

                if (persist != null) {
                    if (pArgs.Persist == false && pArgs.Cleanup == false) { throw new PersistAssistException("[-] Specify persistence or cleanup"); }

                    if (pArgs.Persist) { WriteLine(persist.PersistExec(pArgs)); return; }
                    if (pArgs.Cleanup) { WriteLine(persist.PersistCleanup(pArgs)); return; }
                } else { WriteLine(tradecraft.TradecraftTask(pArgs)); return; }
        }

        public static void listModules() {
            WriteLine("[*] Available modules:\n");
            
            foreach (Persist persist in Lists._persist) {
                switch (persist.PersistCategory) {
                    case Enums.PersistType.Registry:
                        Lists._persistRegistry.Add(persist); break;
                    case Enums.PersistType.MSBuild: 
                        Lists._persistMSBuild.Add(persist); break; 
                    case Enums.PersistType.AccountOperations:
                        Lists._persistAccountOps.Add(persist); break;
                    case Enums.PersistType.Misc:
                        Lists._persistMisc.Add(persist); break;
                    default:
                        break;
                }
            }

            WriteLine("Persistence:\n============");

            WriteLine("Registry:");
            foreach(Persist regPersist in Lists._persistRegistry) { WriteLine($"\t{regPersist.PersistName} - {regPersist.PersistDesc}"); }

            WriteLine("MSBuild:");
            foreach(Persist msbuildPersist in Lists._persistMSBuild) { WriteLine($"\t{msbuildPersist.PersistName} - {msbuildPersist.PersistDesc}"); }

            WriteLine("AccountOperations:");
            foreach (Persist accopsPersist in Lists._persistAccountOps) { WriteLine($"\t{accopsPersist.PersistName} - {accopsPersist.PersistDesc}"); }

            WriteLine("Misc:");
            foreach(Persist miscPersist in Lists._persistMisc) { WriteLine($"\t{miscPersist.PersistName} - {miscPersist.PersistDesc}"); }

            WriteLine("\nTradecraft:\n===========");
            foreach(Tradecraft tradecraft in Lists._tradecraft) { WriteLine($"\t{tradecraft.TradecraftName} - {tradecraft.TradecraftDesc}"); }

            WriteLine("\nPayloads:\n=========");
            foreach(Payload payload in Lists._payloads) { WriteLine($"\t{payload.PayloadName} - {payload.PayloadDesc}"); }
        }
        
        public static void moduleDetails(Persist persist) {
            
            WriteLine(
                $"Name:     {persist.PersistName}\n" +
                $"Desc:     {persist.PersistDesc}\n" +
                $"Usage:    {persist.PersistUsage}\n" +
                $"Category: {persist.PersistCategory}\n" +
                $"Author:   {persist.Author}\n"
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

        public static void helpMenu(OptionSet opts) {
            WriteLine(
                "Usage: PersistAssist.exe -t [technique] -<extra options>\n" +
                "Provide the persist technique and what to do with the technique (persist, cleanup, display info)\n" +
                "To list all available persistence techiques, use PersistAssist.exe -l"
                );
            opts.WriteOptionDescriptions(Console.Out);
        }

        public static void PayloadInit() {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()) {
                if (type.IsSubclassOf(typeof(Payload))) {
                    Payload payload = (Payload)Activator.CreateInstance(type);
                    Lists._payloads.Add(payload);
                }
            }
        }

        public static void PersistInit() {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()) {
                if (type.IsSubclassOf(typeof(Persist))) {
                    Persist persist = (Persist)Activator.CreateInstance(type);
                    Lists._persist.Add(persist);
                }
            }
        }

        public static void TradecraftInit() {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()) {
                if (type.IsSubclassOf(typeof(Tradecraft))) {
                    Tradecraft tradecraft = (Tradecraft)Activator.CreateInstance(type);
                    Lists._tradecraft.Add(tradecraft);
                }
            }
        }

        public static void FunctionInit() {
            if (Lists._persist.Count == 0) { PersistInit(); }
            if (Lists._tradecraft.Count == 0) { TradecraftInit(); }
            if (Lists._payloads.Count == 0) { PayloadInit(); }
        }
    }
}