using YumeScript.Configuration;
using YumeScript.Runtime.InstructionEvaluators;
using YumeScript.Script;
using YumeScript.Tools;
using YumeScript.Translator.External;

namespace YumeScript.Parser.InstructionParsers;

public class CodeInstructionParser : IInstructionParser
{

    private string _appendingConstant = string.Empty;
    
    private readonly IScriptBuilder _scriptBuilder = null!;

    public CodeInstructionParser() { }

    public CodeInstructionParser(IScriptBuilder scriptBuilder)
    {
        _scriptBuilder = scriptBuilder;
    }
    
    public int GetPriority()
    {
        return 0;
    }

    public ParserResult ParseLineTokens(int lineId, string[] tokens)
    {
        if (tokens[0] == "$")
        {
            var op = _scriptBuilder.Allocate(string.Join(' ', tokens, 1, tokens.Length - 1));
            return ParserHelper.Result(new ScriptInstruction(typeof(CodeEvaluator), op, Constants.RegistryAddressResult));
        }

        if (tokens[0] == "$:")
        {
            return ParserHelper.Empty;
        }
        
        return ParserHelper.Skip;
    }

    public ParserResult InterceptLineTokens(int lineId, string[] tokens, int relativeIndentionLevel)
    {
        _appendingConstant += string.Join(' ', tokens) + "\n";
        
        return ParserHelper.Empty;
    }

    public FinalizationParserResult FinalizeIndentionSection(int lineId, string[] tokens)
    {
        var op = _scriptBuilder.Allocate(_appendingConstant);

        return ParserHelper.Discard(new ScriptInstruction(typeof(CodeEvaluator), op));
    }
}