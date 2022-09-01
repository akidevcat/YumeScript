using YumeScript.Exceptions.Parser;
using YumeScript.Script;
using YumeScript.Tools;

namespace YumeScript.Parser;

public class ScriptFunctionBuilder : List<ScriptInstruction>
{
    public readonly string Name;
    
    public ScriptFunctionBuilder(string name)
    {
        if (!NamingHelper.IsFunctionNameValid(name))
        {
            throw new InvalidFunctionNameException();
        }

        Name = name;
    }

    public ScriptFunction Build()
    {
        var result = new ScriptFunction(Name)
        {
            Instructions = ToArray()
        };
        return result;
    }
}