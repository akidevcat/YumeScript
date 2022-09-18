using YumeScript.Configuration;
using YumeScript.Exceptions.Parser;
using YumeScript.Runtime.InstructionEvaluators;
using YumeScript.Script;
using YumeScript.Tools;
using YumeScript.Translator.Exceptions;
using YumeScript.Translator.External;

namespace YumeScript.Parser.InstructionParsers;

public class ConditionInstructionParser : IInstructionParser
{
    private readonly IScriptBuilder _scriptBuilder = null!;
    private readonly List<int> _exitJumpInstructions = new();
    private int _lastJumpFalseInstruction;

    public ConditionInstructionParser() { }
    
    public ConditionInstructionParser(IScriptBuilder scriptBuilder)
    {
        _scriptBuilder = scriptBuilder;
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
            _lastJumpFalseInstruction = instructionId + 1;
            
            var ptrCondition = _scriptBuilder.Allocate(string.Join(' ', tokens, 1, tokens.Length - 1)); // ToDo remove :
            return ParserHelper.Result(new ScriptInstruction(typeof(CodeEvaluator), ptrCondition, Constants.RegistryAddressResult), 
                new ScriptInstruction(typeof(JumpIfFalseEvaluator), Constants.RegistryAddressResult)); 
        }

        if (tokens[0] == "$elif")
        {
            throw new TranslationException(); // ToDo
        }
        
        if (tokens[0] == "$else")
        {
            throw new TranslationException(); // ToDo
        }

        return ParserHelper.Skip;
    }

    public ParserResult InterceptLineTokens(int instructionId, string[] tokens, int relativeIndentionLevel)
    {
        return ParserHelper.Skip;
    }

    public FinalizationParserResult FinalizeIndentionSection(int instructionId, string[] tokens)
    {
        if (tokens.Length > 0 && tokens[0] == "$elif")
        {
            // Add jump to end
            // Add jump_if_false to next block / end
            
            // Allocate condition string
            var opA = _scriptBuilder.Allocate(string.Join(' ', tokens, 1, tokens.Length - 1)); // ToDo remove :
            
            // Set previous jump_if_false pointer to this jump_if_false
            _scriptBuilder.Update(_lastJumpFalseInstruction, x =>
            {
                x.OpB = instructionId + 1;
                return x;
            }, this);
            _lastJumpFalseInstruction = instructionId + 2;
            
            var ptrJump = _scriptBuilder.Append(new ScriptInstruction(typeof(JumpEvaluator)), this);
            _scriptBuilder.Append(new ScriptInstruction(typeof(CodeEvaluator), opA, Constants.RegistryAddressResult), this);
            _scriptBuilder.Append(new ScriptInstruction(typeof(JumpIfFalseEvaluator), Constants.RegistryAddressResult), this);
            _exitJumpInstructions.Add(ptrJump);
        }
        
        if (tokens.Length > 0 && tokens[0] == "$else:")
        {
            // Set previous jump_if_false pointer to this block
            _scriptBuilder.Update(_lastJumpFalseInstruction, x =>
            {
                x.OpB = instructionId + 1;
                return x;
            }, this);
            _lastJumpFalseInstruction = -1;

            var ptrJump = _scriptBuilder.Append(new ScriptInstruction(typeof(JumpEvaluator)), this);
            _exitJumpInstructions.Add(ptrJump);
            
            return ParserHelper.Keep();
        }

        foreach (var i in _exitJumpInstructions)
        {
            _scriptBuilder.Update(i, x =>
            {
                x.OpB = instructionId;
                return x;
            }, this);
        }

        if (_lastJumpFalseInstruction != -1)
        {
            _scriptBuilder.Update(_lastJumpFalseInstruction, x =>
            {
                x.OpB = instructionId + 1;
                return x;
            }, this);
            _lastJumpFalseInstruction = -1;
        }
        
        return ParserHelper.Discard(new ScriptInstruction(typeof(PointerEvaluator))); // Block (Frame) exit instruction - just as a pointer
    }
}