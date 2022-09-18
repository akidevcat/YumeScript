using System.Collections.Immutable;
using System.ComponentModel;
using YumeScript.SDK.Exceptions;
using YumeScript.SDK.Tools;

namespace YumeScript.SDK.Script;

[ImmutableObject(true)]
[Serializable]
public class ScriptFile
{
    #region Serializable Fields
    
    /// <summary>
    /// Full name of this <see cref="ScriptFile"/>
    /// </summary>
    public readonly string FullName;
    /// <summary>
    /// Contained functions of this <see cref="ScriptFile"/>
    /// </summary>
    public readonly ImmutableArray<ScriptFunction> Functions;
    
    #endregion
    
    #region Non-Serializable Fields
    
    /// <summary>
    /// Mapping function indices with function names
    /// </summary>
    [NonSerialized]
    private readonly ImmutableDictionary<string, int> _functionsMap;
    
    #endregion

    public ScriptFile(string fullName, IEnumerable<ScriptFunction> functions)
    {
        if (!NamingHelper.IsFullNameValid(fullName))
        {
            throw new InvalidFullNameException(fullName);
        }
        
        FullName = fullName;
        Functions = functions.ToImmutableArray();

        // Prepare functions map
        var i = 0;
        _functionsMap = functions.ToImmutableDictionary(k => k.Name, _ => i++);
    }
}