using YumeScript.Exceptions.Parser;
using YumeScript.External;
using YumeScript.Script;
using YumeScript.Tools;

namespace YumeScript.Parser.InstructionParsers;

public class ConditionInstructionParser : IInstructionParser // ToDo Rework
{

    private string _mainData = string.Empty;
    private IScriptTree _scriptTree = null!;
    private int _initialInstructionId = 0;

    public ConditionInstructionParser() { }
    
    public ConditionInstructionParser(IScriptTree scriptTree)
    {
        _scriptTree = scriptTree;
    }
    
    public int GetPriority()
    {
        return -1;
    }

    public IEnumerable<RuntimeInstruction>? ParseLineTokens(int instructionId, string[] tokens)
    {
        if (tokens[0] == "$if")
        {
            _initialInstructionId = instructionId;
            return ParserHelper.GetInstructionResult(string.Join(' ', tokens, 1, tokens.Length - 1)); // ToDo remove :
        }

        if (tokens[0] == "$elif")
        {
            throw new ParserException(); // ToDo
        }
        
        if (tokens[0] == "$else")
        {
            throw new ParserException(); // ToDo
        }

        return ParserHelper.SkipResult;
    }

    public IEnumerable<RuntimeInstruction>? InterceptLineTokens(int instructionId, string[] tokens)
    {
        return ParserHelper.SkipResult;
    }

    public (bool, IEnumerable<RuntimeInstruction>) FinalizeIndentionSection(int instructionId, string[] tokens)
    {
        // ToDo else:
        
        if (tokens[0] == "$elif")
        {
            // ToDo return jump instruction
            var instruction = _scriptTree[_initialInstructionId]!.Value;
            instruction.MainData += "\n" + string.Join(' ', tokens, 1, tokens.Length - 1);
            _scriptTree[_initialInstructionId] = instruction;
            
            return (true, Array.Empty<RuntimeInstruction>());
        }
        
        if (tokens[0] == "$else")
        {
            // ToDo return jump instruction
            var instruction = _scriptTree[_initialInstructionId]!.Value;
            instruction.MainData += "\n" + string.Join(' ', tokens, 1, tokens.Length - 1);
            _scriptTree[_initialInstructionId] = instruction;
            
            return (true, Array.Empty<RuntimeInstruction>());
        }

        return (false, Array.Empty<RuntimeInstruction>());
    }
}