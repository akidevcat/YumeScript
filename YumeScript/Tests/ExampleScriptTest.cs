using YumeScript.Extensions;

namespace YumeScript.Tests;

public class ExampleScriptTest
{
    public void Run()
    {
        var runtime = new RuntimeConfiguration(cfg =>
        {
            cfg.UseCallbackEngine(new DebugCallbackEngine());
        }).CreateRuntime();
    }
}