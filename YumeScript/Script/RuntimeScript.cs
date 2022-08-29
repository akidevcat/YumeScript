using YumeScript.Configuration;
using YumeScript.Exceptions;
using YumeScript.Tools;

namespace YumeScript.Script;

public class RuntimeScript
{
    public string FullName { get; private set; }
    internal List<string>? SourceCode;
    internal RuntimeFunction[]? Functions;

    public bool IsParsed => Functions != null;
    
    public RuntimeScript()
    {
        FullName = Constants.RuntimeScriptDefaultName;
    }

    public RuntimeScript(string fullName, string sourceCode) : this(fullName, sourceCode.Split('\n')) { }
    
    public RuntimeScript(string fullName, IEnumerable<string> sourceCode) : this()
    {
        if (!NamingHelper.IsFullNameValid(fullName))
        {
            throw new InvalidFullNameException(fullName);
        }
        
        FullName = fullName;
        // Copy source code
        SourceCode = new List<string>(sourceCode);
    }
}