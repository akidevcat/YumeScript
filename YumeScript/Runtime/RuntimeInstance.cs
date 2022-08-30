using YumeScript.Configuration;
using YumeScript.Exceptions;
using YumeScript.External;
using YumeScript.Parser;
using YumeScript.Script;
using YumeScript.Tools;

namespace YumeScript.Runtime;

public class RuntimeInstance
{
    internal readonly SortedList<int, Type> InstructionParsers;
    internal readonly Dictionary<int, IInstructionEvaluator> InstructionEvaluators;
    internal readonly Dictionary<string, RuntimeScript> ScriptLibrary;
    internal readonly RuntimeThread? Thread;
    internal bool FlSkipUnknownInstructions;

    internal RuntimeInstance(SortedList<int, Type> instructionParsers, Dictionary<int, IInstructionEvaluator> instructionEvaluators)
    {
        InstructionParsers = instructionParsers;
        InstructionEvaluators = instructionEvaluators;
        ScriptLibrary = new Dictionary<string, RuntimeScript>();
    }

    public bool AddScript(RuntimeScript script) => ScriptLibrary.TryAdd(script.FullName, script);

    public bool RemoveScript(string fullName)
    {
        if (!NamingHelper.IsFullNameValid(fullName))
        {
            throw new InvalidFullNameException(fullName);
        }

        return ScriptLibrary.Remove(fullName);
    }

    public void ParseScripts()
    {
        foreach (var script in ScriptLibrary.Values)
        {
            if (script.IsParsed)
                continue;
            
            this.ParseScript(script);
        }
    }
    
    
}