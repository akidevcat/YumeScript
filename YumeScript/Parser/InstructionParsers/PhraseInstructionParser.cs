using YumeScript.External;
using YumeScript.Runtime.InstructionEvaluators;
using YumeScript.Script;
using YumeScript.Tools;

namespace YumeScript.Parser.InstructionParsers;

public class PhraseInstructionParser : IInstructionParser
{
    
    private readonly IScriptTree _scriptTree = null!;
    
    public PhraseInstructionParser() { }
    
    public PhraseInstructionParser(IScriptTree scriptTree)
    {
        _scriptTree = scriptTree;
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
                charTextPtr = _scriptTree.Allocate(token[1..^1]);
                charTextTokenIndex = i;
                break;
            }
        }

        if (charTextPtr == null)
        {
            return ParserHelper.Skip;
        }

        var args = tokens.Where((x, i) => i != charTextTokenIndex);
        var argsPtr = _scriptTree.Allocate(string.Join(" ", args)); //ToDo allocate array instead of string
        
        return ParserHelper.Result(new ScriptInstruction(typeof(PrintPhraseEvaluator), charTextPtr.Value, argsPtr)); // PhrasePtr, ArgsPtr
    }

    public ParserResult InterceptLineTokens(int instructionId, string[] tokens)
    {
        throw new NotImplementedException();
    }

    public FinalizationParserResult FinalizeIndentionSection(int instructionId, string[] tokens)
    {
        throw new NotImplementedException();
    }
}