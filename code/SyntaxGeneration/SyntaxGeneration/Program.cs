using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SyntaxGeneration.Builder;

namespace SyntaxGeneration
{
    class Program
    {
        public Program()
        {
            
        }
        static void Main(string[] args)
        {
            //create main class tomorrow to output as console application
            var codeGen = new CodeGeneratorPlayground();

            Console.WriteLine("Generating your code please wait");
            var newNode = codeGen.GenerateComplexCode();

            Console.WriteLine(newNode);

            var assemblyName = Path.GetRandomFileName();
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.GetExecutingAssembly().Location),
                MetadataReference.CreateFromFile(Assembly.ReflectionOnlyLoad("Microsoft.Azure.WebJobs").Location)
            };

            var compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { newNode.SyntaxTree },
                references: references.ToArray(),
                options: new CSharpCompilationOptions(OutputKind.ConsoleApplication));


            using (var memoryStream = new MemoryStream())
            {
                var directory = Directory.GetCurrentDirectory();
                var path = Path.Combine(directory, "test");
                var result = compilation.Emit(path);

                if (!result.Success)
                {
                    var failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (var diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
                else
                {
                    Console.WriteLine("Getting result of method");
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    var assembly = Assembly.Load(memoryStream.ToArray());

                    var type = assembly.GetType("MyNameSpace.Program");
                    
                    var returnResult = (int)type.InvokeMember("GetCalculatedPrice",
                        BindingFlags.Default | BindingFlags.InvokeMethod,
                        null,
                        null,
                        new object[] { 18, 5 });
                }
            }
            Console.WriteLine("\n \n \n");
            Console.WriteLine(newNode);
            Console.ReadLine();
        }
    }
}