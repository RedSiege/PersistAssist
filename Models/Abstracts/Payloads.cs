namespace PersistAssist.Models
{
    public abstract class Payload
    {
        public abstract string PayloadName { get; }
        public abstract string PayloadDesc { get; }
        public abstract string[] PayloadNamespaces { get; }
        public abstract string PayloadCode { get; }
        public virtual string Author { get; }
    }
}
