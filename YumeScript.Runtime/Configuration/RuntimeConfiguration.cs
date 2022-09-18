using YumeScript.External;
using YumeScript.Parser;

namespace YumeScript.Configuration;

public class RuntimeConfiguration
{
    public ICallbackEngine? CallbackEngine { get; private set; }
    public InstructionSetConfiguration InstructionSetConfiguration { get; private set; } = InstructionSetConfiguration.UseDefaultSet;
    public HashSet<Type> InstructionTypes { get; private set; }
    public bool FlSkipUnknownInstructions { get; private set; }

    public RuntimeConfiguration(Action<RuntimeConfiguration> cfg)
    {
        cfg(this);
        InstructionTypes = new HashSet<Type>();
    }

    public RuntimeConfiguration UseCallbackEngine(ICallbackEngine callbackEngine)
    {
        CallbackEngine = callbackEngine;
        return this;
    }
    
    public RuntimeConfiguration UseInstructionSet(InstructionSetConfiguration configuration)
    {
        InstructionSetConfiguration = configuration;
        return this;
    }

    public RuntimeConfiguration SkipUnknownInstructions(bool state = true)
    {
        FlSkipUnknownInstructions = state;
        return this;
    }
    
    public bool AppendInstructionParser<T>() where T : IInstructionParser => InstructionTypes.Add(typeof(T));

    public bool AppendInstructionEvaluator<T>() where T : IInstructionEvaluator => InstructionTypes.Add(typeof(T));
}