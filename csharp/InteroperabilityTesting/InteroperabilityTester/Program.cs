using System;
using System.Collections.Generic;
using System.Text;

namespace InteroperabilityTester
{
    class Program
    {

        static void Main(string[] args)
        {

            if (args.Length == 0 || (args.Length == 1 && args[0].Equals("help")))
            {
                PrintHelp();
                return;
            }

            if (args.Length == 1 && args[0].Equals("serialize"))
            {
                Console.WriteLine("Serializing reference messages...");
                SerializeCommand.Execute();
                return;
            }

            if (args.Length > 2 &&
                args[0].Equals("execute") &&
                args[1].Equals("ctors"))
            {
                Console.WriteLine("Executing client to reference server test case...");
                int port = 1255;
                if (args.Length > 3)
                {
                    port = Convert.ToInt16(args[3]);
                }

                ExecuteClientToReferenceServerTestCaseCommand.Execute(args[2], port);

                return;
            }

            PrintHelp();
        }

        public static void PrintHelp()
        {
            Console.WriteLine("Interoperability tester use: iotester command");
            Console.WriteLine("Commands:");
            Console.WriteLine("  1) serialize - Serializes messages to zip file.");
            Console.WriteLine("  1) execute ctors ip [port] - Executes client to reference server test case.");
        }

    }
}
