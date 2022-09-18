namespace YumeScript.Demo;

public static class Program
{
    public static void Main(string[] args)
    {
        // var runtimeCfg = new RuntimeConfiguration(cfg =>
        // {
        //     cfg
        //         .UseCallbackEngine(new ConsoleCallbackEngine())
        //         .UseInstructionSet(InstructionSetConfiguration.UseDefaultSet)
        //         .SkipUnknownInstructions();
        // });
        //
        // var runtime = runtimeCfg.CreateRuntime();
        //
        // var script = new Script.Script("Example", File.ReadAllLines("../../../Resources/Example.yume"));
        // runtime.AddScript(script);
        // runtime.ParseScripts();
        //
        // Console.WriteLine(runtime.ConvertScriptToString(script));
    }
}