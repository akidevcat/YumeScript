using System.Collections.Immutable;
using YumeScript.Runtime.InstructionEvaluators;
using YumeScript.Script;
using YumeScript.Tools;
using YumeScript.Translator.External;

namespace YumeScript.Parser.InstructionParsers;

public class PhraseInstructionParser : IInstructionParser
{
    
    private readonly IScriptBuilder _scriptBuilder = null!;
    
    public PhraseInstructionParser() { }
    
    public PhraseInstructionParser(IScriptBuilder scriptBuilder)
    {
        _scriptBuilder = scriptBuilder;
    }
    
    public int GetPriority()
    {
        return 10;
    }

    public ParserResult ParseLineTokens(int instructionId, string[] tokens)
    {
        // Check if tokens contain a string token - then parse as a character line instruction
        int? charTextPtr = null;
        var charTextTokenIndex = 0;

        for (var i = 0; i < tokens.Length; i++)
        {
            var token = tokens[i];
            if ((token.StartsWith("\"") && token.EndsWith("\"")) || (token.StartsWith("'") && token.EndsWith("'")))
            {
                charTextPtr = _scriptBuilder.Allocate(token[1..^1]);
                charTextTokenIndex = i;
                break;
            }
        }

        if (charTextPtr == null)
        {
            return ParserHelper.Skip;
        }

        var args = tokens.Where((x, i) => i != charTextTokenIndex).ToArray();
        var argsPtr = _scriptBuilder.Allocate(args);
        
        return ParserHelper.Result(new ScriptInstruction(typeof(PrintPhraseEvaluator), charTextPtr.Value, argsPtr)); // PhrasePtr, ArgsPtr
    }

    public ParserResult InterceptLineTokens(int instructionId, string[] tokens, int relativeIndentionLevel)
    {
        throw new NotImplementedException();
    }

    public FinalizationParserResult FinalizeIndentionSection(int instructionId, string[] tokens)
    {
        throw new NotImplementedException();
    }
}