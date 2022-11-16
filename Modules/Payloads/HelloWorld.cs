using PersistAssist.Models;

namespace PersistAssist.Modules.Payloads
{
    class HelloWorld : Payload
    {
        public override string PayloadName => "HelloWorld";
        public override string PayloadDesc => "hola mundo";
        public override Data.Enums.PayloadLang PayloadLanguage => Data.Enums.PayloadLang.CSharp;
        public override string[] PayloadNamespaces => new string[] { "System" };
        public override string PayloadCode => @"
            Console.WriteLine(""Hello World"");
        ";


    }
}
