using YumeScript.Script;

namespace YumeScript.External;

/// <summary>
/// 
/// </summary>
public interface IScriptTree
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="lineId"></param>
    ScriptInstruction? this[int lineId]
    {
        get;
        set;
    }

    /// <summary>
    /// 
    /// </summary>
    int Length
    {
        get;
    }
    
    /// <summary>
    /// Appends a new constant to script file
    /// </summary>
    /// <param name="constant">Any serializable object</param>
    /// <returns>Constant's hash value for referencing</returns>
    int Allocate(object constant);
}