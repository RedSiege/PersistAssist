using System;

namespace PersistAssist.Models
{
    class PersistAssistException : Exception
    {
        public PersistAssistException(string message) : base(message) { }

        public PersistAssistException(string message, Exception inner) : base(message, inner) { }
    }
}
