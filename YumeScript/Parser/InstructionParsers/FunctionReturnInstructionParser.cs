using YumeScript.Parser;
using YumeScript.Script;
using YumeScript.Tools;

namespace YumeScript.Exceptions.Parser;

public class FunctionReturnInstructionParser : IInstructionParser
{
    public int GetPriority()
    {
        return 2;
    }

    public ParserResult ParseLineTokens(int lineId, string[] tokens)
    {
        if (tokens[0] == "<-")
        {
            if (tokens.Length != 1)
            {
                throw new ParserException(); //ToDo Change exception
            }

            return ParserHelper.Result(new ScriptInstruction(null));
        }

        return ParserHelper.Skip;
    }

    public ParserResult InterceptLineTokens(int lineId, string[] tokens)
    {
        throw new ParserException();
    }

    public FinalizationParserResult FinalizeIndentionSection(int lineId, string[] tokens)
    {
        throw new ParserException();
    }
}