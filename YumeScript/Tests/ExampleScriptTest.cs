using NUnit.Framework;
using YumeScript.Configuration;
using YumeScript.Extensions;
using YumeScript.External;
using YumeScript.Script;
using YumeScript.Tools;

namespace YumeScript.Tests;

[TestFixture]
public class ExampleScriptTest
{
    [Test]
    public void Run()
    {
        var runtimeCfg = new RuntimeConfiguration(cfg =>
        {
            cfg
                .UseCallbackEngine(new DebugCallbackEngine())
                .UseInstructionSet(InstructionSetConfiguration.UseDefaultSet)
                .SkipUnknownInstructions();
        });
        
        var runtime = runtimeCfg.CreateRuntime();

        var script = new Script.Script("Example", File.ReadAllLines("../../../Resources/Example.yume"));
        runtime.AddScript(script);
        runtime.ParseScripts();
        
        DebugHelper.PrintScriptInstructions(runtime, script);
    }
}