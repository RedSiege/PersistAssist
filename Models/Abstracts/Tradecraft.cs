namespace PersistAssist.Models
{
    public abstract class Tradecraft
    {
        public abstract string TradecraftName { get; }
        public abstract string TradecraftDesc { get; }
        public abstract string TradecraftUsage { get; }
        public virtual string Author { get; }
        public abstract string TradecraftTask(ParsedArgs pArgs);
    }
}
