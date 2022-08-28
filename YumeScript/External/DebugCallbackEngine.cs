namespace YumeScript;

public class DebugCallbackEngine : ICallbackEngine
{
    public object? CallStatement(string source)
    {
        Console.WriteLine("--- Callback Engine Code Called ---");
        Console.WriteLine(source);
        Console.WriteLine("--- End ---");
        return null;
    }
}