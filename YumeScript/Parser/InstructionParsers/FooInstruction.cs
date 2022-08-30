using YumeScript.Script;

namespace YumeScript.Parser.InstructionParsers;

public class FooInstruction : IInstructionParser
{
    public FooInstruction()
    {
        
    }
    
    public int GetPriority()
    {
        return 100;
    }

    public IEnumerable<RuntimeInstruction>? ParseLineTokens(string[] tokens)
    {
        return Array.Empty<RuntimeInstruction>();
    }

    public IEnumerable<RuntimeInstruction>? InterceptLineTokens(string[] tokens)
    {
        return Array.Empty<RuntimeInstruction>();
    }

    public IEnumerable<RuntimeInstruction> FinalizeIndentionSection()
    {
        return Array.Empty<RuntimeInstruction>();
    }
}