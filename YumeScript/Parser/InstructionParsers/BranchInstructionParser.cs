using YumeScript.Configuration;
using YumeScript.External;
using YumeScript.Runtime.InstructionEvaluators;
using YumeScript.Script;
using YumeScript.Tools;

namespace YumeScript.Parser.InstructionParsers;

public class BranchInstructionParser : IInstructionParser
{
    private int _readBranchInstructionId = -1;
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
        if (tokens[0] == "?" && tokens[^1].EndsWith(':'))
        {
            var args = tokens.Skip(1).ToArray();
            args[^1] = args[^1][..(args[^1].Length - 1)];

            var ptrArgs = _scriptTree.Allocate(args);
            
            _readBranchInstructionId = instructionId;
            return ParserHelper.Result(new ScriptInstruction(typeof(ReadBranchEvaluator), ptrArgs, Constants.RegistryAddressResult));
        }

        return ParserHelper.Skip;
    }

    public ParserResult InterceptLineTokens(int instructionId, string[] tokens)
    {
        return ParserHelper.Empty;
    }

    public FinalizationParserResult FinalizeIndentionSection(int instructionId, string[] tokens)
    {
        throw new NotImplementedException();
    }
}