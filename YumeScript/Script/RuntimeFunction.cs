namespace YumeScript.Script;

public class RuntimeFunction
{
    public string Name;
    internal RuntimeInstruction[] Instructions;

    public RuntimeFunction(string name) : this(name, Array.Empty<RuntimeInstruction>()) { }
    
    public RuntimeFunction(string name, RuntimeInstruction[] instructions)
    {
        Name = name;
        Instructions = (RuntimeInstruction[]) instructions.Clone();
    }
}