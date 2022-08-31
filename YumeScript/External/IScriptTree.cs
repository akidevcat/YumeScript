using YumeScript.Script;

namespace YumeScript.External;

public interface IScriptTree
{
    RuntimeInstruction? this[int lineId]
    {
        get;
        set;
    }

    int Length
    {
        get;
    }
}