using System.Text;
using System.Diagnostics;

namespace PersistAssist.Utils
{
    public class Procs
    {
        // https://github.com/Gr1mmie/AtlasC2/blob/master/Implant/Tasks/Execute/Administration/Ps.cs
        public static string listProcs() {

            StringBuilder outProcs = new StringBuilder();

            Process[] procs = Process.GetProcesses();

            int procIDLen = getMaxProcIDLen(procs);
            int procNameLen = getMaxProcNameLen(procs) + procIDLen;
            int procSessionIDLen = getMaxProcSessionIDLen(procs) + procNameLen;

            outProcs.AppendLine($"{"PID".Align(procIDLen)} {"ProcName".Align(procNameLen)} {"SessionId".Align(procSessionIDLen)}");
            outProcs.AppendLine($"{"---".Align(procIDLen)} {"--------".Align(procNameLen)} {"---------".Align(procSessionIDLen)}");

            foreach (Process proc in procs) {
                outProcs.AppendLine($"{proc.Id.Align(procIDLen)} {proc.ProcessName.Align(procNameLen)} {proc.SessionId.Align(procSessionIDLen)}");
            }

            return outProcs.ToString();
        }

        public static string listProcs(string procName) {

            StringBuilder outProcs = new StringBuilder();

            Process[] procs = Process.GetProcesses();

            int procIDLen = getMaxProcIDLen(procs);
            int procNameLen = getMaxProcNameLen(procs) + procIDLen;
            int procSessionIDLen = getMaxProcSessionIDLen(procs) + procNameLen;

            outProcs.AppendLine($"{"PID".Align(procIDLen)} {"ProcName".Align(procNameLen)} {"SessionId".Align(procSessionIDLen)}");
            outProcs.AppendLine($"{"---".Align(procIDLen)} {"--------".Align(procNameLen)} {"---------".Align(procSessionIDLen)}");

            foreach (Process proc in procs) {
                if (proc.ProcessName == procName) {
                    outProcs.AppendLine($"{proc.Id.Align(procIDLen)} {proc.ProcessName.Align(procNameLen)} {proc.SessionId.Align(procSessionIDLen)}");
                }
            }

            return outProcs.ToString();
        }

        private static int getMaxProcIDLen(Process[] procs) {
            var maxProcIDLen = 0;
            foreach (Process proc in procs) {
                if (proc.Id.ToString().Length > maxProcIDLen) { maxProcIDLen = proc.Id.ToString().Length; }
            }

            return maxProcIDLen;
        }

        private static int getMaxProcNameLen(Process[] procs) {
            int maxProcNameLen = 0;
            foreach (Process proc in procs) {
                if (proc.ProcessName.Length > maxProcNameLen) { maxProcNameLen = proc.ProcessName.Length; }
            }

            return maxProcNameLen;
        }

        private static int getMaxProcSessionIDLen(Process[] procs) {
            int maxProcSessionIDLen = 0;
            foreach (Process proc in procs) {
                if (proc.SessionId.ToString().Length > maxProcSessionIDLen) { maxProcSessionIDLen = proc.SessionId.ToString().Length; }
            }

            return maxProcSessionIDLen;
        }

    }
}
