using YumeScript.Script;

namespace YumeScript.External;

public interface IInstructionEvaluator
{
    public Task Evaluate(ScriptInstruction instruction);
}