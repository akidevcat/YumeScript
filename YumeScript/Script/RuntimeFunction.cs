using YumeScript.Exceptions.Parser;
using YumeScript.Tools;

namespace YumeScript.Script;

public class RuntimeFunction
{
    public string Name;
    internal RuntimeInstruction[] Instructions;

    public RuntimeFunction(string name) : this(name, Array.Empty<RuntimeInstruction>()) { }
    
    public RuntimeFunction(string name, RuntimeInstruction[] instructions)
    {
        if (!NamingHelper.IsFunctionNameValid(name))
        {
            throw new InvalidFunctionNameException();
        }
        
        Name = name;
        Instructions = (RuntimeInstruction[]) instructions.Clone();
    }
}