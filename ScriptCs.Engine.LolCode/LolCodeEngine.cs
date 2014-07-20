using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using notdot.LOLCode;
using ScriptCs.Contracts;

namespace ScriptCs.Engine.LolCode
{
    public class LolCodeEngine : IScriptEngine
    {
        public ScriptResult Execute(string code, string[] scriptArgs, AssemblyReferences references, IEnumerable<string> namespaces,
            ScriptPackSession scriptPackSession)
        {
            var debugLine = code.Substring(0, code.IndexOf(".lol\"") + 5);
            code = code.Replace(debugLine, string.Empty).TrimStart();

            var compiler = new LOLCodeCodeProvider();
            var cparam = new CompilerParameters
            {
                GenerateInMemory = true,
                MainClass = "Program",
                OutputAssembly = "lolcode.dll"
            };
            cparam.ReferencedAssemblies.AddRange(references.PathReferences != null ? references.PathReferences.ToArray() : new string[0]);
            var results = compiler.CompileAssemblyFromSource(cparam, code);

            var x = results.CompiledAssembly.GetReferencedAssemblies();

            var startupType = results.CompiledAssembly.GetType("Program", true, true);
            var instance = Activator.CreateInstance(startupType, false);
            var entryPoint = results.CompiledAssembly.EntryPoint;

            var result = entryPoint.Invoke(instance, new object[] { });

            return new ScriptResult(result);
        }

        public string BaseDirectory { get; set; }
        public string CacheDirectory { get; set; }
        public string FileName { get; set; }
    }
}
