namespace YumeScript.External;

public class DebugCallbackEngine : ICallbackEngine
{
    public Task<object?> CallStatement(string source)
    {
        Console.WriteLine("--- Callback Engine Code Called ---");
        Console.WriteLine(source);
        return Task.FromResult<object?>(null);
    }

    public Task PrintPhrase(string charName, string charText, string[] args)
    {
        Console.WriteLine($"--- {charName} ---");
        Console.WriteLine(charText);
        return Task.FromResult<object?>(null);
    }

    public Task<int> AwaitBranching(string[] options, string[] args)
    {
        Console.WriteLine("--- ? ---");
        Console.WriteLine(string.Join('\n', options));
        string? response;
        var responseParsed = 0;
        do
        {
            response = Console.ReadLine();
        } while (response != null && int.TryParse(response, out responseParsed) && 
                 responseParsed < options.Length && responseParsed >= 0);
        
        return Task.FromResult(responseParsed);
    }
}