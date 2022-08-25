﻿using PersistAssist.Models;

using static PersistAssist.Models.Data.Enums;

namespace PersistAssist.Functions
{
    public class InlineTasks : Persist
    {
        public override string PersistName => "InlineTasks";

        public override string PersistDesc => "Deploys MSBuild InlineTask based payload. Drops file to disk";

        public override string PersistUsage => "PersistAssist.exe -t InlineTasks ";

        public override PersistType PersistCategory => PersistType.MSBuild;

        public override string PersistCleanup(ParsedArgs pArgs)
        {
            try {
                throw new System.NotImplementedException();
            } catch (System.Exception e) { return $"PersistAssist module failed: {e.Message}"; }
        }

        public override string PersistExec(ParsedArgs pArgs)
        {
            try {
                throw new System.NotImplementedException();
            } catch (System.Exception e) { return $"PersistAssist module failed: {e.Message}"; }
        }
    }
}
