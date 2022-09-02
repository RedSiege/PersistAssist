namespace PersistAssist.Models.Templates
{
    internal class HelloWorld : Payload
    {
        public override string PayloadName => "HelloWorld";

        public override string PayloadDesc => "hola mundo";
        public override string[] PayloadNamespaces => new string[] { "System" };
        public override string PayloadCode => @"
            Console.Writeline(""Hello World"");
        ";

    }
}
