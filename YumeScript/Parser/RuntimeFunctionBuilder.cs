using YumeScript.Exceptions.Parser;
using YumeScript.Script;
using YumeScript.Tools;

namespace YumeScript.Parser;

public class RuntimeFunctionBuilder : List<RuntimeInstruction>
{
    public readonly string Name;
    
    public RuntimeFunctionBuilder(string name)
    {
        if (!NamingHelper.IsFunctionNameValid(name))
        {
            throw new InvalidFunctionNameException();
        }

        Name = name;
    }

    public RuntimeFunction Build()
    {
        var result = new RuntimeFunction(Name)
        {
            Instructions = ToArray()
        };
        return result;
    }
}