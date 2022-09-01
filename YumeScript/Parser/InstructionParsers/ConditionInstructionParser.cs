using YumeScript.Exceptions.Parser;
using YumeScript.External;
using YumeScript.Runtime.InstructionEvaluators;
using YumeScript.Script;
using YumeScript.Tools;

namespace YumeScript.Parser.InstructionParsers;

public class ConditionInstructionParser : IInstructionParser
{
    private readonly IScriptTree _scriptTree = null!;
    private List<int> _exitJumpInstructions = new();
    private int _lastJumpFalseInstruction = 0;
    private string _blockToken = string.Empty;

    public ConditionInstructionParser() { }
    
    public ConditionInstructionParser(IScriptTree scriptTree)
    {
        _scriptTree = scriptTree;
    }
    
    public int GetPriority()
    {
        return -1;
    }

    public ParserResult ParseLineTokens(int instructionId, string[] tokens)
    {
        if (tokens[0] == "$if")
        {
            // Add jump_if_false into next block / end
            _lastJumpFalseInstruction = instructionId;
            
            var opA = _scriptTree.Allocate(string.Join(' ', tokens, 1, tokens.Length - 1)); // ToDo remove :
            return ParserHelper.Result(new ScriptInstruction(typeof(JumpIfFalseEvaluator), opA)); 
        }

        if (tokens[0] == "$elif")
        {
            throw new ParserException(); // ToDo
        }
        
        if (tokens[0] == "$else")
        {
            throw new ParserException(); // ToDo
        }

        return ParserHelper.Skip;
    }

    public ParserResult InterceptLineTokens(int instructionId, string[] tokens)
    {
        return ParserHelper.Skip;
    }

    public FinalizationParserResult FinalizeIndentionSection(int instructionId, string[] tokens)
    {
        if (tokens[0] == "$elif")
        {
            // Add jump to end
            // Add jump_if_false to next block / end
            
            // Allocate condition string
            var opA = _scriptTree.Allocate(string.Join(' ', tokens, 1, tokens.Length - 1)); // ToDo remove :
            _exitJumpInstructions.Add(instructionId);

            // Set previous jump_if_false pointer to this jump_if_false
            var lastJumpFalse = _scriptTree[_lastJumpFalseInstruction]!.Value;
            lastJumpFalse.OpB = instructionId + 1;
            _scriptTree[_lastJumpFalseInstruction] = lastJumpFalse;
            
            _lastJumpFalseInstruction = instructionId;
            return ParserHelper.Keep(new ScriptInstruction(typeof(JumpEvaluator)), new ScriptInstruction(typeof(JumpIfFalseEvaluator), opA)); // jump, jump_if_false
        }
        
        if (tokens[0] == "$else:")
        {
            // Add jump to end
            
            _exitJumpInstructions.Add(instructionId);
            
            // Set previous jump_if_false pointer to this block
            var lastJumpFalse = _scriptTree[_lastJumpFalseInstruction]!.Value;
            lastJumpFalse.OpB = instructionId + 1;
            _scriptTree[_lastJumpFalseInstruction] = lastJumpFalse;
            
            return ParserHelper.Keep(new ScriptInstruction(typeof(JumpEvaluator))); // jump
        }

        foreach (var i in _exitJumpInstructions)
        {
            var temp = _scriptTree[i]!.Value;
            temp.OpB = instructionId;
            _scriptTree[i] = temp;
        }

        return ParserHelper.Discard(new ScriptInstruction(typeof(PointerEvaluator))); // Block (Frame) exit instruction - just as a pointer
    }
}