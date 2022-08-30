using NUnit.Framework;
using YumeScript.Configuration;
using YumeScript.Extensions;
using YumeScript.External;
using YumeScript.Script;

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

        runtime.AddScript(new RuntimeScript("Example", File.ReadAllLines("../../../Resources/Example.yume")));
        
        runtime.ParseScripts();
    }
}