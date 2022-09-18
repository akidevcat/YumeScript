using System.Collections;
using System.Collections.Immutable;
using System.ComponentModel;
using YumeScript.SDK.Exceptions;
using YumeScript.SDK.Tools;

namespace YumeScript.SDK.Script;

[ImmutableObject(true)]
[Serializable]
public class ScriptFunction : ICloneable
{
    public readonly string Name;
    public readonly ImmutableArray<ScriptInstruction> Instructions;

    public ScriptFunction(string name, IEnumerable<ScriptInstruction> instructions)
    {
        if (!NamingHelper.IsFunctionNameValid(name))
        {
            throw new InvalidFunctionNameException();
        }
        
        Name = name;
        Instructions = instructions.ToImmutableArray();
    }
    
    public int Count => Instructions.Length;
    public ScriptInstruction this[int index] => Instructions[index];
    
    public object Clone() => new ScriptFunction(Name, Instructions);
}