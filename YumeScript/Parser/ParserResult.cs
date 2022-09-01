using YumeScript.Script;

namespace YumeScript.Parser;

public class ParserResult
{
    internal readonly IEnumerable<ScriptInstruction>? Instructions = null;

    internal ParserResult()
    {
        
    }
    
    internal ParserResult(IEnumerable<ScriptInstruction> instructions)
    {
        Instructions = instructions;
    }
}