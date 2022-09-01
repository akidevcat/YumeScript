using System.Collections.Immutable;
using System.Runtime.Serialization;
using YumeScript.Configuration;
using YumeScript.Exceptions;
using YumeScript.Tools;

namespace YumeScript.Script;

public class Script
{
    public readonly string FullName;
    internal readonly List<string>? SourceCode;
    internal ImmutableDictionary<string, ScriptFunction>? Functions;
    internal ImmutableDictionary<int, IConvertible>? OpConstants;

    public bool IsParsed => Functions != null && OpConstants != null;
    
    public Script()
    {
        FullName = Constants.RuntimeScriptDefaultName;
    }

    public Script(string fullName, string sourceCode) : this(fullName, sourceCode.Split('\n')) { }
    
    public Script(string fullName, IEnumerable<string> sourceCode) : this()
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