using YumeScript.External;
using YumeScript.Tools;

namespace YumeScript.Script;

public struct ScriptInstruction
{
    public int Type;
    public int OpA;
    public int OpB;

    public ScriptInstruction(Type? type, int opA = 0, int opB = 0)
    {
        if (type != null && !type.IsAssignableTo(typeof(IInstructionEvaluator)))
        {
            throw new ArgumentException("Instruction Evaluator has an invalid type");
        }
        
        Type = type?.GetHashCode() ?? 0;
        OpA = opA;
        OpB = opB;
    }
}