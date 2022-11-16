using static PersistAssist.Models.Data.Enums;

namespace PersistAssist.Models
{
    public abstract class Payload
    {
        public abstract string PayloadName { get; }
        public abstract string PayloadDesc { get; }
        public abstract PayloadLang PayloadLanguage { get; }
        public virtual string[] PayloadNamespaces { get; }
        public virtual string DLLImports { get; }
        public abstract string PayloadCode { get; }
        public virtual string Author { get; }
    }
}
