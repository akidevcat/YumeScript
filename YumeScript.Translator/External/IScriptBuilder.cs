using YumeScript.SDK.Script;

namespace YumeScript.Translator.External;

/// <summary>
/// 
/// </summary>
public interface IScriptBuilder
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="lineId"></param>
    ScriptInstruction? this[int lineId]
    {
        get;
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

    int Append(IEnumerable<ScriptInstruction> instructions, object? caller = null);
    
    int Append(ScriptInstruction instruction, object? caller = null);
    
    int Insert(IEnumerable<ScriptInstruction> instructions, int startAt, object? caller = null);
    
    int Insert(ScriptInstruction instruction, int startAt, object? caller = null);

    void Update(int index, Func<ScriptInstruction, ScriptInstruction> updateFunc, object? caller = null);
}