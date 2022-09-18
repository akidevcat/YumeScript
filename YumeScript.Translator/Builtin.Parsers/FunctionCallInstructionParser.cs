using YumeScript.Exceptions.Parser;
using YumeScript.Script;
using YumeScript.Tools;
using YumeScript.Translator.Exceptions;
using YumeScript.Translator.External;

namespace YumeScript.Parser.InstructionParsers;

public class FunctionCallInstructionParser : IInstructionParser
{
    
    private readonly IScriptBuilder _scriptBuilder = null!;
    
    public FunctionCallInstructionParser() { }
    
    public FunctionCallInstructionParser(IScriptBuilder scriptBuilder)
    {
        _scriptBuilder = scriptBuilder;
    }
    
    public int GetPriority()
    {
        return 1;
    }

    public ParserResult ParseLineTokens(int lineId, string[] tokens)
    {
        if (tokens[0] == "->")
        {
            // ToDo check call name validity and throw exception

            if (tokens.Length != 2)
            {
                throw new InvalidFunctionCallName();
            }

            var op = _scriptBuilder.Allocate(tokens[1]);
            return ParserHelper.Result(new ScriptInstruction(null, op));
        }

        return ParserHelper.Skip;
    }

    public ParserResult InterceptLineTokens(int lineId, string[] tokens, int relativeIndentionLevel)
    {
        throw new TranslationException();
    }

    public FinalizationParserResult FinalizeIndentionSection(int lineId, string[] tokens)
    {
        throw new TranslationException();
    }
}