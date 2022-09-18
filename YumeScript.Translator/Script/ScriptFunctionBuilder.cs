using YumeScript.Exceptions.Parser;
using YumeScript.Script;
using YumeScript.Tools;

namespace YumeScript.Parser;

public class ScriptFunctionBuilder : List<(object?, ScriptInstruction)>, ICloneable
{
    public readonly string Name;
    public readonly int Pointer;
    
    public ScriptFunctionBuilder(string name, int pointer)
    {
        if (!NamingHelper.IsFunctionNameValid(name))
        {
            throw new InvalidFunctionNameException();
        }

        Name = name;
        Pointer = pointer;
    }

    public ScriptFunction Build()
    {
        var result = new ScriptFunction(Name, Pointer)
        {
            Instructions = this.Select(x => x.Item2).ToArray()
        };
        return result;
    }

    public object Clone()
    {
        var result = new ScriptFunctionBuilder(Name, Pointer);
        result.AddRange(this);
        return result;
    }
}