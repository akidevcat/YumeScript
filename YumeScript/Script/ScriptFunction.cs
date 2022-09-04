using YumeScript.Exceptions.Parser;
using YumeScript.Tools;

namespace YumeScript.Script;

public class ScriptFunction
{
    public readonly string Name;
    public readonly int Pointer;
    internal ScriptInstruction[] Instructions;

    public ScriptFunction(string name, int pointer) : this(name, pointer, Array.Empty<ScriptInstruction>()) { }
    
    public ScriptFunction(string name, int pointer, ScriptInstruction[] instructions)
    {
        if (!NamingHelper.IsFunctionNameValid(name))
        {
            throw new InvalidFunctionNameException();
        }
        
        Name = name;
        Pointer = pointer;
        Instructions = (ScriptInstruction[]) instructions.Clone();
    }
}