using YumeScript.Script;
using YumeScript.Tools;

namespace YumeScript.Parser.InstructionParsers;

public class FooInstructionParser : IInstructionParser
{
    public int GetPriority()
    {
        return 100;
    }

    public ParserResult ParseLineTokens(int instructionId, string[] tokens)
    {
        return ParserHelper.Skip;
        return ParserHelper.Empty;
    }

    public ParserResult InterceptLineTokens(int instructionId, string[] tokens, int relativeIndentionLevel)
    {
        return ParserHelper.Empty;
    }

    public FinalizationParserResult FinalizeIndentionSection(int instructionId, string[] tokens)
    {
        return ParserHelper.Discard();
    }
}