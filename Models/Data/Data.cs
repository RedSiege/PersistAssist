using System.Collections.Generic;

namespace PersistAssist.Models
{
    public class Data
    {
        public static string version = "v0.2";
        public static bool displayBanner = true;

        public static string banner =
        " ______                  __       __   _______             __       __   \n" +
        "|   __ .-----.----.-----|__.-----|  |_|   _   .-----.-----|__.-----|  |_ \n" +
        "|    __|  -__|   _|__ --|  |__ --|   _|       |__ --|__ --|  |__ --|   _|\n" +
        "|___|  |_____|__| |_____|__|_____|____|___|___|_____|_____|__|_____|____|\n" +
        $"   Author: @Grimmie (@FortyNorthSec)\n" +
        $"      Ver: {version}\n";

        public class Lists
        {
            public static readonly List<Persist> _persist = new List<Persist>();
            public static readonly List<Tradecraft> _tradecraft = new List<Tradecraft>();
            public static readonly List<Payload> _payloads = new List<Payload>();

        }

        public class Enums
        {
            public enum PersistType
            {
                Registry,
                MSBuild,
                AccountOperations,
                WMI,
                Misc
            }

            public enum PayloadLang
            {
                CSharp,
                VBA
            }

            public enum RegistryContext
            {
                HKLM,
                HKCU
            }
            public enum ErrorCodes
            {
                LOGON_FAILED = 1326
            }

        }
    }
}
