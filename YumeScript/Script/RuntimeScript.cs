using System.Collections;
using YumeScript.Configuration;
using YumeScript.Exceptions;
using YumeScript.Tools;

namespace YumeScript;

public class RuntimeScript
{
    public string FullName { get; private set; }
    private List<string> _sourceCode;

    public RuntimeScript()
    {
        FullName = Constants.RuntimeScriptDefaultName;
        _sourceCode = new List<string>();
    }
    
    public RuntimeScript(string fullName, IEnumerable<string> sourceCode) : this()
    {
        if (!NamingHelper.IsFullNameValid(fullName))
        {
            throw new InvalidFullNameException(fullName);
        }
        
        FullName = fullName;
        // Copy source code
        _sourceCode = new List<string>(sourceCode);
    }
}