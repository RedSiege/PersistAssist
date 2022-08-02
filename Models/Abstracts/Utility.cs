namespace PersistAssist.Models
{
    public abstract class Utility
    {
        public abstract string UtilName { get; }
        public abstract string UtilDesc { get; }
        public abstract string UtilTask(ParsedArgs pArgs);
    }
}
