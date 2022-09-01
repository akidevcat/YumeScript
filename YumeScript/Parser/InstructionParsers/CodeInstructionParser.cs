using YumeScript.External;
using YumeScript.Script;
using YumeScript.Tools;

namespace YumeScript.Parser.InstructionParsers;

public class CodeInstructionParser : IInstructionParser
{

    private string _appendingConstant = string.Empty;
    
    private readonly IScriptTree _scriptTree = null!;

    public CodeInstructionParser() { }

    public CodeInstructionParser(IScriptTree scriptTree)
    {
        _scriptTree = scriptTree;
    }
    
    public int GetPriority()
    {
        return 0;
    }

    public ParserResult ParseLineTokens(int lineId, string[] tokens)
    {
        if (tokens[0] == "$")
        {
            var op = _scriptTree.Allocate(string.Join(' ', tokens, 1, tokens.Length - 1));
            return ParserHelper.Result(new ScriptInstruction(null, op));
        }

        if (tokens[0] == "$:")
        {
            return ParserHelper.Empty;
        }
        
        return ParserHelper.Skip;
    }

    public ParserResult InterceptLineTokens(int lineId, string[] tokens)
    {
        _appendingConstant += string.Join(' ', tokens) + "\n";
        
        return ParserHelper.Empty;
    }

    public FinalizationParserResult FinalizeIndentionSection(int lineId, string[] tokens)
    {
        var op = _scriptTree.Allocate(_appendingConstant);

        return ParserHelper.Discard(new ScriptInstruction(null, op));
    }
}