using YumeScript.Script;

namespace YumeScript.Tools;

public static class ParserHelper
{
    
    public static IEnumerable<RuntimeInstruction>? GetInstructionResult<T>(string mainData)
    {
        return new[]
        {
            new RuntimeInstruction(typeof(T), mainData)
        };
    }
    
    public static IEnumerable<RuntimeInstruction>? GetInstructionResult(string mainData) //ToDo Remove
    {
        return new[]
        {
            new RuntimeInstruction(null, mainData)
        };
    }

    public static IEnumerable<RuntimeInstruction>? EmptyResult => Array.Empty<RuntimeInstruction>();
    
    public static IEnumerable<RuntimeInstruction>? SkipResult => null;
}