using YumeScript.Exceptions.InstructionParsers;
using YumeScript.Exceptions.Parser;
using YumeScript.External;
using YumeScript.Script;
using YumeScript.Tools;

namespace YumeScript.Parser.InstructionParsers;

public class FunctionCallInstructionParser : IInstructionParser
{
    
    private readonly IScriptTree _scriptTree = null!;
    
    public FunctionCallInstructionParser() { }
    
    public FunctionCallInstructionParser(IScriptTree scriptTree)
    {
        _scriptTree = scriptTree;
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

            var op = _scriptTree.Allocate(tokens[1]);
            return ParserHelper.Result(new ScriptInstruction(null, op));
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