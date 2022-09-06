using NUnit.Framework;
using YumeScript.Configuration;
using YumeScript.Extensions;
using YumeScript.External;

namespace YumeScript.Tests;

[TestFixture]
public class BranchInstructionTest
{
    [Test]
    public void Run()
    {
        var runtimeCfg = new RuntimeConfiguration(cfg =>
        {
            cfg
                .UseCallbackEngine(new ConsoleCallbackEngine())
                .UseInstructionSet(InstructionSetConfiguration.UseDefaultSet)
                .SkipUnknownInstructions();
        });
        
        var runtime = runtimeCfg.CreateRuntime();

        var script = new Script.Script("Tests.BranchInstructionTest", File.ReadAllLines("../../../Resources/BranchInstructionTest.yume"));
        runtime.AddScript(script);
        runtime.ParseScripts();
        
        Console.WriteLine(runtime.ConvertScriptToString(script));
    }
}