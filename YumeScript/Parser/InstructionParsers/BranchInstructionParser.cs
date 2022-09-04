using YumeScript.External;
using YumeScript.Runtime.InstructionEvaluators;
using YumeScript.Script;
using YumeScript.Tools;

namespace YumeScript.Parser.InstructionParsers;

public class BranchInstructionParser : IInstructionParser
{
    private readonly IScriptTree _scriptTree = null!;

    public BranchInstructionParser() { }

    public BranchInstructionParser(IScriptTree scriptTree)
    {
        _scriptTree = scriptTree;
    }

    public int GetPriority()
    {
        return 7;
    }

    public ParserResult ParseLineTokens(int instructionId, string[] tokens)
    {
        if (tokens[0] == "?")
        {
            return ParserHelper.Result(new ScriptInstruction(typeof(ReadBranchEvaluator), (int)0x0FFFFFFF));
        }

        return ParserHelper.Skip;
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