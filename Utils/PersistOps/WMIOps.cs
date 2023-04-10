using System;
using System.Linq;
using System.Text;

using System.Management;

namespace PersistAssist.Utils
{
    public class WMIOps
    {

        // return all properties
        public static string Query(string query)
        {

            StringBuilder sb = new StringBuilder();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject obj in searcher.Get())
            {
                foreach (PropertyData prop in obj.Properties)
                {
                    sb.AppendLine($"{prop.Name} : {prop.Value}");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        //return specified properties
        public static string Query(string query, string[] properties)
        {
            StringBuilder sb = new StringBuilder();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject obj in searcher.Get())
            {
                foreach (PropertyData prop in obj.Properties)
                {
                    if (properties.Contains(prop.Name)) { sb.AppendLine($"{prop.Name} : {prop.Value}"); }
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static ManagementObject registerEventFilter(string query, string eventName)
        {

            ManagementClass eventFilterWMI = new ManagementClass(new ManagementScope(@"\\.\root\subscription"), new ManagementPath("__EventFilter"), null);

            WqlEventQuery eventQuery = new WqlEventQuery(query);

            ManagementObject eventFilter = eventFilterWMI.CreateInstance();

            eventFilter["Name"] = eventName;
            eventFilter["Query"] = eventQuery.QueryString;
            eventFilter["QueryLanguage"] = eventQuery.QueryLanguage;
            eventFilter["EventNamespace"] = @"\root\cimv2";

            eventFilter.Put();

            return eventFilter;
        }

        public static ManagementClass registerActiveScriptEventConsumer(string eventConsumerName, string scriptContents)
        {

            ManagementClass eventConsumer = new ManagementClass(new ManagementScope(@"\\.\root\subscription"), new ManagementPath("ActiveScriptEventConsumer"), null);
            eventConsumer.CreateInstance();

            eventConsumer["Name"] = eventConsumerName;
            eventConsumer["ScriptingEngine"] = "VBScript";
            eventConsumer["ScriptText"] = scriptContents;

            eventConsumer.Put();

            return eventConsumer;
        }

        public static ManagementObject registerCommandLineEventConsumer(string eventConsumerName, string execPath)
        {
            try
            {
                ManagementObject eventConsumer = new ManagementClass(new ManagementScope(@"\\.\root\subscription"), new ManagementPath("CommandLineEventConsumer"), null)
                .CreateInstance();

                eventConsumer["Name"] = eventConsumerName;
                eventConsumer["ExecutablePath"] = execPath;
                eventConsumer["CommandLineTemplate"] = execPath;

                eventConsumer.Put();

                return eventConsumer;
            }
            catch (Exception e) { System.Console.WriteLine(e); return new ManagementClass(); }
        }

        public static ManagementObject registerFilterToConsumerBinding(string filterPath, string consumerPath)
        {
            try
            {
                ManagementObject consumerBinding = new ManagementClass(new ManagementScope(@"\\.\root\subscription"), new ManagementPath("__FilterToConsumerBinding"), null)
                .CreateInstance();

                consumerBinding["Filter"] = filterPath;
                consumerBinding["Consumer"] = consumerPath;

                consumerBinding.Put();

                return consumerBinding;
            }
            catch (Exception e) { System.Console.WriteLine(e); return new ManagementObject(); }
        }

        public static ManagementObject registerIntervalTimerInstruction(string timerName, int timeBetween)
        {
            ManagementObject timerInterval = new ManagementClass(new ManagementScope(@"\\.\root\subscription"), new ManagementPath("IntervalTimerInstruction"), null)
                .CreateInstance();

            timerInterval["TimerId"] = timerName;
            timerInterval["IntervalBetweenEvents"] = timeBetween;

            timerInterval.Put();

            return timerInterval;
        }
    }
}
