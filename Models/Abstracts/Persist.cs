using static PersistAssist.Models.Data.Enums;

namespace PersistAssist.Models
{
    public abstract class Persist
    {
        public abstract string PersistName { get; }
        public abstract string PersistDesc { get; }
        public abstract string PersistUsage { get; }
        public abstract PersistType PersistCategory { get; }
        public abstract string PersistExec(ParsedArgs pArgs);
        public abstract string PersistCleanup(ParsedArgs pArgs);
        public virtual string Author { get; }
        public virtual bool requiresAdmin { get; }
    }
}
