using YumeScript.Script;
using YumeScript.Tools;

namespace YumeScript.Parser.InstructionParsers;

public class CodeInstructionParser : IInstructionParser
{

    private string _scriptCode = "";

    public int GetPriority()
    {
        return 0;
    }

    public IEnumerable<RuntimeInstruction>? ParseLineTokens(int lineId, string[] tokens)
    {
        if (tokens[0] == "$")
        {
            return ParserHelper.GetInstructionResult(string.Join(' ', tokens, 1, tokens.Length - 1));
        }

        if (tokens[0] == "$:")
        {
            return ParserHelper.EmptyResult;
        }

        return null;
    }

    public IEnumerable<RuntimeInstruction>? InterceptLineTokens(int lineId, string[] tokens)
    {
        _scriptCode += string.Join(' ', tokens) + "\n";
        
        return ParserHelper.EmptyResult;
    }

    public (bool, IEnumerable<RuntimeInstruction>) FinalizeIndentionSection(int lineId, string[] tokens)
    {
        return (false, new[]
        {
            new RuntimeInstruction(null, _scriptCode)
        });
    }
}