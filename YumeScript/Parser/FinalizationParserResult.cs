using YumeScript.Script;

namespace YumeScript.Parser;

public class FinalizationParserResult : ParserResult
{
    internal readonly bool KeepParser = false;

    internal FinalizationParserResult(IEnumerable<ScriptInstruction> instructions, bool keepParser = false) : base(instructions)
    {
        KeepParser = keepParser;
    }
}