using YumeScript.Script;

namespace YumeScript.Parser.InstructionParsers;

public class FooInstructionParser : IInstructionParser
{
    public FooInstructionParser()
    {
        
    }
    
    public int GetPriority()
    {
        return 100;
    }

    public IEnumerable<RuntimeInstruction>? ParseLineTokens(int lineId, string[] tokens)
    {
        return Array.Empty<RuntimeInstruction>();
    }

    public IEnumerable<RuntimeInstruction>? InterceptLineTokens(int lineId, string[] tokens)
    {
        return Array.Empty<RuntimeInstruction>();
    }

    public (bool, IEnumerable<RuntimeInstruction>) FinalizeIndentionSection(int lineId, string[] tokens)
    {
        return (false, Array.Empty<RuntimeInstruction>());
    }
}