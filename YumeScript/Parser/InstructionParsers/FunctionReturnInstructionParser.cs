using YumeScript.Parser;
using YumeScript.Script;

namespace YumeScript.Exceptions.Parser;

public class FunctionReturnInstructionParser : IInstructionParser
{
    public int GetPriority()
    {
        return 2;
    }

    public IEnumerable<RuntimeInstruction>? ParseLineTokens(int lineId, string[] tokens)
    {
        if (tokens[0] == "<-")
        {
            if (tokens.Length != 1)
            {
                throw new ParserException(); //ToDo Change exception
            }
            
            return new[]
            {
                new RuntimeInstruction(null, string.Empty)
            };
        }

        return null;
    }

    public IEnumerable<RuntimeInstruction>? InterceptLineTokens(int lineId, string[] tokens)
    {
        throw new ParserException();
    }

    public (bool, IEnumerable<RuntimeInstruction>) FinalizeIndentionSection(int lineId, string[] tokens)
    {
        throw new ParserException();
    }
}