using YumeScript.Configuration;
using YumeScript.Exceptions.Parser;
using YumeScript.Runtime.InstructionEvaluators;
using YumeScript.Script;
using YumeScript.Tools;
using YumeScript.Translator.Exceptions;
using YumeScript.Translator.External;

namespace YumeScript.Parser.InstructionParsers;

public class BranchInstructionParser : IInstructionParser
{
    private int _readBranchInstructionId = -1;
    private readonly List<string> _branchingOptions = new();
    private readonly IScriptBuilder _scriptBuilder = null!;

    public BranchInstructionParser() { }

    public BranchInstructionParser(IScriptBuilder scriptBuilder)
    {
        _scriptBuilder = scriptBuilder;
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

            var ptrArgs = _scriptBuilder.Allocate(args);
            
            _readBranchInstructionId = instructionId;
            return ParserHelper.Result(new ScriptInstruction(typeof(ReadBranchEvaluator), ptrArgs, Constants.RegistryAddressResult));
        }

        return ParserHelper.Skip;
    }

    public ParserResult InterceptLineTokens(int instructionId, string[] tokens, int relativeIndentionLevel)
    {
        // Should be a branching option
        if (relativeIndentionLevel == 1)
        {
            if (!tokens[^1].EndsWith(':') || tokens.Length != 1)
            {
                throw new TranslationException(); //ToDo
            }

            if (tokens[0][0] != '\'' && tokens[0][0] != '"')
            {
                throw new TranslationException(); //ToDo
            }
            
            if (tokens[0][^2] != '\'' && tokens[0][^2] != '"')
            {
                throw new TranslationException(); //ToDo
            }
            
            _branchingOptions.Add(tokens[0][1..^2]);
            return ParserHelper.Empty;
        }

        return ParserHelper.Skip;
    }

    public FinalizationParserResult FinalizeIndentionSection(int instructionId, string[] tokens)
    {
        // Allocate branching options
        var ptrBranchingOptions = _scriptBuilder.Allocate(_branchingOptions.ToArray());
        
        // Change options operator on ReadBranch
        _scriptBuilder.Update(_readBranchInstructionId, x =>
        {
            x.OpC = ptrBranchingOptions;
            return x;
        }, this);

        return ParserHelper.Discard();
    }
}