using YumeScript.Parser;
using YumeScript.Script;

namespace YumeScript.Tools;

public static class ParserHelper
{
    public static ParserResult Skip => new();
    
    public static ParserResult Empty => new(Array.Empty<ScriptInstruction>());

    public static ParserResult Result(params ScriptInstruction[] instructions)
    {
        return new ParserResult(instructions);
    }

    public static FinalizationParserResult Keep(params ScriptInstruction[] instructions)
    {
        return new FinalizationParserResult(instructions, true);
    }
    
    public static FinalizationParserResult Discard(params ScriptInstruction[] instructions)
    {
        return new FinalizationParserResult(instructions);
    }
}