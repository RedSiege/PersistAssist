using System;

namespace PersistAssist.Models
{
    public class ParsedArgs
    {
        public string Technique { get; set; }
        public string Action { get; set; }
        public string Search { get; set; }
        public string listModule { get; set; }

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
        public string userName { get; set; }
        public string Passwd { get; set; }
        public string domain { get; set; }
        public string Query { get; set; }
        public string Command { get; set; }
        public string eventFilterName { get; set; }
        public string eventFilterQuery { get; set; }
        public string eventConsumerName { get; set; }
        public string eventConsumerValue { get; set; }

        public bool Persist = false;
        public bool Cleanup = false;
        public bool listModules = false;
        public bool moduleInfo = false;
        public bool Help = false;
    }
}
