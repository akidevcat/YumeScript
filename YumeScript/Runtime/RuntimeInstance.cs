using YumeScript.Configuration;
using YumeScript.Exceptions;
using YumeScript.Parser;
using YumeScript.Script;
using YumeScript.Tools;

namespace YumeScript.Runtime;

public class RuntimeInstance
{
    private RuntimeConfiguration _configuration;
    private readonly Dictionary<string, RuntimeScript> _scriptLibrary;
    private readonly RuntimeThread? _thread;

    internal RuntimeInstance(RuntimeConfiguration configuration)
    {
        _configuration = configuration;
        _scriptLibrary = new Dictionary<string, RuntimeScript>();
    }

    public bool AddScript(RuntimeScript script) => _scriptLibrary.TryAdd(script.FullName, script);

    public bool RemoveScript(string fullName)
    {
        if (!NamingHelper.IsFullNameValid(fullName))
        {
            throw new InvalidFullNameException(fullName);
        }

        return _scriptLibrary.Remove(fullName);
    }

    public void ParseScripts()
    {
        foreach (var script in _scriptLibrary.Values)
        {
            if (script.IsParsed)
                continue;
            
            RuntimeParser.ParseScript(script);
        }
    }
    
    
}