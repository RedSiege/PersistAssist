using System.Text;
using System.Collections.Generic;
using System.Security.Principal;

using Microsoft.Win32.TaskScheduler;

//using Microsoft.Win32.TaskScheduler;

namespace PersistAssist.Utils
{
    public class SchTaskOps
    {
        public static string CreateTask() {
            return "";
        }

        public static string RemoveTask() {
            return "";
        }

        public static string ModifyTask()
        {
            return "";
        }

        public static string ListTasks()
        {
            StringBuilder outTasks = new StringBuilder();

            TaskService taskSvc = new TaskService();

            IEnumerable<Task> tasks = taskSvc.AllTasks;

            int taskLen = getMaxTNLen(tasks);

            outTasks.AppendLine($"{"Task Name".Align(taskLen)} {"State"}");
            outTasks.AppendLine($"{"---------".Align(taskLen)} {"------"}");

            foreach (Task task in tasks)
            {
                if (task.IsActive) { outTasks.AppendLine($"{task.Name.Align(taskLen)} {task.State}"); }
            }

            return outTasks.ToString();
        }

        public static string TaskInfo(string taskName)
        {
            StringBuilder outTasks = new StringBuilder();

            TaskService taskSvc = new TaskService();

            IEnumerable<Task> tasks = taskSvc.AllTasks;

            int taskLen = getMaxTNLen(tasks);

            foreach (Task task in tasks)
            {
                if (task.Name == taskName)
                {

                    if (!task.IsActive) { return $"[-] Task {task.Name} inactive"; }
                    else
                    {
                        TriggerCollection triggers = task.Definition.Triggers;

                        ActionCollection actions = task.Definition.Actions;

                        SecurityIdentifier SI = task.SecurityDescriptor.Owner;
                        NTAccount owner = SI.Translate(typeof(NTAccount)) as NTAccount;

                        if (triggers[0].TriggerType.ToString() == "Time")
                        {
                            outTasks.AppendLine(
                                $"Name: {task.Name.Align(taskLen)}\n" +
                                $"Status: {task.State}\n" +
                                $"Path: {task.Folder.Path}\n" +
                                $"Owner: {owner}\n" +
                                $"Trigger(s): \n\tTriggerType: {triggers[0].TriggerType}\n\tExecTime: {task.NextRunTime}\n" +
                                $"Action: {task.Definition.Actions[0]}\n");
                        }
                        else
                        {
                            outTasks.AppendLine(
                                $"Name: {task.Name.Align(taskLen)}\n" +
                                $"Status: {task.State}\n" +
                                $"Path: {task.Folder.Path}\n" +
                                $"Owner: {owner}\n" +
                                $"Trigger(s): \n\tTriggerType: {triggers[0].TriggerType}\n" +
                                $"Action: {task.Definition.Actions[0]}\n");
                        }
                    }
                }
            }

            return outTasks.ToString();
        }

        private static int getMaxTNLen(IEnumerable<Task> tasks)
        {
            var maxLen = 0;
            foreach (Task task in tasks)
            {
                if (task.Name.ToString().Length > maxLen) { maxLen = task.Name.ToString().Length; }
            }

            return maxLen;
        }
    }
}
