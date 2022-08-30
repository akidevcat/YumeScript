using YumeScript.Script;

namespace YumeScript.Parser.InstructionParsers;

public class CodeInstruction : IInstructionParser
{

    private string _scriptCode = "";

    public int GetPriority()
    {
        return 0;
    }

    public IEnumerable<RuntimeInstruction>? ParseLineTokens(string[] tokens)
    {
        if (tokens[0] == "$")
        {
            return new[]
            {
                new RuntimeInstruction(this, string.Join(' ', tokens, 1, tokens.Length - 1))
            };
        }

        if (tokens[0] == "$:")
        {
            return Array.Empty<RuntimeInstruction>();
        }

        return null;
    }

    public IEnumerable<RuntimeInstruction>? InterceptLineTokens(string[] tokens)
    {
        _scriptCode += string.Join(' ', tokens) + "\n";
        
        return Array.Empty<RuntimeInstruction>();
    }

    public IEnumerable<RuntimeInstruction> FinalizeIndentionSection()
    {
        return new[]
        {
            new RuntimeInstruction(this, _scriptCode)
        };
    }
}