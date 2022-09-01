using YumeScript.Exceptions.Parser;
using YumeScript.Tools;

namespace YumeScript.Script;

public class ScriptFunction
{
    public string Name;
    internal ScriptInstruction[] Instructions;

    public ScriptFunction(string name) : this(name, Array.Empty<ScriptInstruction>()) { }
    
    public ScriptFunction(string name, ScriptInstruction[] instructions)
    {
        if (!NamingHelper.IsFunctionNameValid(name))
        {
            throw new InvalidFunctionNameException();
        }
        
        Name = name;
        Instructions = (ScriptInstruction[]) instructions.Clone();
    }
}