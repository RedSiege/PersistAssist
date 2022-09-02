using System;

namespace PersistAssist.Models
{
    public class ParsedArgs
    {
        public string Technique { get; set; }
        public string Action { get; set; }
        public string Search { get; set; }
        public string regRootKey { get; set; }
        public string regSubKey { get; set; }
        public string regKeyValue { get; set; }
        public string regContext { get; set; }
        public string Payload { get; set; }
        public string TaskName { get; set; }
        public string filePath { get; set; }
        public string dupPath { get; set; }
        public string newTime { get; set; }
        public string Timestamp { get; set; }

        public bool Persist = false;
        public bool Cleanup = false;
        public bool listModules = false;
        public bool moduleInfo = false;
        public bool Help = false;
    }
}
