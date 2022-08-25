using System.Text;

namespace PersistAssist.Utils
{
    public class MSBuildOps
    {
        public static string InlineTemplate = @"
<Project ToolsVersion=""4.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
	<Target Name = ""notsus"" >
        < TaskThingy />
    </ Target >
    < UsingTask TaskName=""TaskThingy"" TaskFactory=""CodeTaskFactory"" AssemblyFile=""C:\Windows\Microsoft.NET\Framework\v4.0.30319\Microsoft.Build.Tasks.v4.0.dll"">
		<Task>	
			<Code Type = ""Class"" Language=""cs"">
				<![CDATA[

[IMPORTED NAMESPACES HERE]

using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

public class TaskThingy : Task, ITask
    {
        public override bool Execute()
        {
            [CODE HERE]

        }
    }
				]]>
			</Code>
		</Task>
	</UsingTask>
</Project>
";

        public static string OverrideTemplate = @"
<Project xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <UsingTask TaskName=""[TASKNAME]""
      TaskFactory=""CodeTaskFactory""
      AssemblyFile=""C:\Windows\Microsoft.Net\Framework\v4.0.30319\Microsoft.Build.Tasks.v4.0.dll"" >

    <ParameterGroup>
      <SourceFiles Required=""true"" ParameterType=""Microsoft.Build.Framework.ITaskItem[]""/>
      <DestinationFiles Output=""true"" ParameterType=""Microsoft.Build.Framework.ITaskItem[]"" />
    </ParameterGroup>

    <Task>
      [NAMESPACES]
      <Code Type=""Fragment"" Language=""cs"">
	<![CDATA[
    [CODE]
	]]>
      </Code>
    </Task>
  </UsingTask>
</Project>
        ";

        public static string TaskTemplate = @"
<Project xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
    <Target Name=""Build"" >

        <Copy SourceFiles=""[TASKNAME].proj"" DestinationFiles=""destination.proj"" />
    </Target >
</Project >
        ";

        public static string NamespaceTemplate = "<Using Namespace=\"[NAMESPACE]\" />";

        public static string MSBuildPath = @"C:\Windows\Microsoft.Net\Framework\v4.0.30319";

        public static string GenTask(string taskName) {
            StringBuilder sb = new StringBuilder();
            sb.Append(TaskTemplate);
            
            sb.Replace("[TASKNAME]", taskName);

            return sb.ToString();
        }

        public static string GenOverride(string taskName, string[] namespaces, string code) {
            StringBuilder sb = new StringBuilder();
            StringBuilder namespace_sb = new StringBuilder();

            sb.Append(OverrideTemplate);

            foreach (string _namespace in namespaces) {
                namespace_sb.AppendLine(NamespaceTemplate);
                namespace_sb.Replace("[NAMESPACE]", _namespace);
            }

            sb.Replace("[NAMESPACES]", namespace_sb.ToString());
            sb.Replace("[TASKNAME]", taskName);
            sb.Replace("[CODE]", code);

            return sb.ToString();
        }

        //https://github.com/cobbr/Covenant/blob/master/Covenant/Models/Launchers/MSBuildLauncher.cs
    }
}