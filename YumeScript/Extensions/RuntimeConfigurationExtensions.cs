using System.Reflection;
using YumeScript.Configuration;
using YumeScript.External;
using YumeScript.Parser;
using YumeScript.Runtime;
using YumeScript.Tools;

namespace YumeScript.Extensions;

public static class RuntimeConfigurationExtensions
{
    public static RuntimeInstance CreateRuntime(this RuntimeConfiguration configuration)
    {
        // ToDo pre-checks

        var evaluators = new Dictionary<int, IInstructionEvaluator>();
        var parsers = new SortedList<int, Type>();

        // Append evaluator and parser instances from internal and external namespaces
        switch (configuration.InstructionSetConfiguration)
        {
            case InstructionSetConfiguration.UseEmptySet:
                break;
            
            case InstructionSetConfiguration.UseDefaultSet:
                AppendDefaultInstructionSet(evaluators, parsers);
                break;

            case InstructionSetConfiguration.UseExtendedSetOnly:
                AppendExtendedInstructionSet(evaluators, parsers);
                break;
            
            case InstructionSetConfiguration.UseExtendedDefaultSet:
                AppendDefaultInstructionSet(evaluators, parsers);
                AppendExtendedInstructionSet(evaluators, parsers);
                break;
        }

        // ToDo Rework
        // Add evaluator and parser instances from configuration types
        // var cfgInstances = ReflectionHelper.CreateTypesInstances(configuration.InstructionTypes);
        //
        // foreach (var x in cfgInstances)
        // {
        //     switch (x)
        //     {
        //         case IInstructionEvaluator e:
        //             evaluators.Add(NamingHelper.GetObjectTypeHashcode(e), e);
        //             break;
        //         case IInstructionParser p:
        //             parsers.Add(p.GetPriority(), p);
        //             break;
        //     }
        // }

        return new RuntimeInstance(parsers, evaluators)
        {
            FlSkipUnknownInstructions = configuration.FlSkipUnknownInstructions
        };
    }

    private static void AppendDefaultInstructionSet(IDictionary<int, IInstructionEvaluator> evaluators, IDictionary<int, Type> parsers)
    {
        var defaultEvaluatorTypes = ReflectionHelper.GetInterfaceSubclassTypes<IInstructionEvaluator>(GetEvaluatorsAssembly());
        var defaultParserTypes = ReflectionHelper.GetInterfaceSubclassTypes<IInstructionParser>(GetParsersAssembly());

        foreach (var e in ReflectionHelper.CreateTypesInstances<IInstructionEvaluator>(defaultEvaluatorTypes))
        {
            evaluators.Add(NamingHelper.GetObjectTypeHashcode(e), e);
        }

        foreach (var p in ReflectionHelper.CreateTypesInstances<IInstructionParser>(defaultParserTypes))
        {
            parsers.Add(p.GetPriority(), p.GetType());
        }
    }
    
    private static void AppendExtendedInstructionSet(IDictionary<int, IInstructionEvaluator> evaluators, IDictionary<int, Type> parsers)
    {
        var externalAssemblies = GetExternalAssemblies().ToList();
        var extendedEvaluatorTypes = externalAssemblies.SelectMany(ReflectionHelper.GetInterfaceSubclassTypes<IInstructionEvaluator>);
        var extendedParserTypes = externalAssemblies.SelectMany(ReflectionHelper.GetInterfaceSubclassTypes<IInstructionParser>);

        foreach (var e in ReflectionHelper.CreateTypesInstances<IInstructionEvaluator>(extendedEvaluatorTypes))
        {
            evaluators.Add(NamingHelper.GetObjectTypeHashcode(e), e);
        }
                
        foreach (var p in ReflectionHelper.CreateTypesInstances<IInstructionParser>(extendedParserTypes))
        {
            parsers.Add(p.GetPriority(), p.GetType());
        }
    }

    private static Assembly GetEvaluatorsAssembly() => typeof(RuntimeInstance).Assembly;
    
    private static Assembly GetParsersAssembly() => typeof(RuntimeParser).Assembly;

    private static IEnumerable<Assembly> GetExternalAssemblies() =>
        AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a != GetEvaluatorsAssembly() && a != GetParsersAssembly());
}