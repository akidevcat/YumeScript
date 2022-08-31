using YumeScript.Exceptions.InstructionParsers;
using YumeScript.Exceptions.Parser;
using YumeScript.Script;
using YumeScript.Tools;

namespace YumeScript.Parser.InstructionParsers;

public class FunctionCallInstructionParser : IInstructionParser
{
    public int GetPriority()
    {
        return 1;
    }

    public IEnumerable<RuntimeInstruction>? ParseLineTokens(int lineId, string[] tokens)
    {
        if (tokens[0] == "->")
        {
            // ToDo check call name validity and throw exception

            if (tokens.Length != 2)
            {
                throw new InvalidFunctionCallName();
            }
            
            return new[]
            {
                new RuntimeInstruction(null, tokens[1])
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