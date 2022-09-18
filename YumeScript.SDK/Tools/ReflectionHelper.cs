using System.Reflection;

namespace YumeScript.Tools;

public static class ReflectionHelper
{
    public static T CreateInjectedInstance<T>(params (Type, object)[] services) => (T)CreateInjectedInstance(typeof(T), services);

    public static object CreateInjectedInstance(Type instanceType, params (Type, object)[] services)
    {
        Dictionary<Type, object> servicesMap;
        try
        {
            servicesMap = services.ToDictionary(k => k.Item1, v => v.Item2);
        }
        catch (ArgumentException)
        {
            throw new ArgumentException("Passed arguments have repeating types");
        }

        var constructors = instanceType.GetConstructors();

        // Find most suitable constructor - the one that has the greatest number of parameters
        var ctor = constructors
            .Where(c => !c.GetParameters().Select(p => p.ParameterType).Except(servicesMap.Keys).Any())
            .MaxBy(c => c.GetParameters().Length);

        if (ctor == null)
        {
            throw new ArgumentException($"ParserType {instanceType.Name} has no suitable constructor to be invoked");
        }
        
        var parameters = ctor.GetParameters().Select(p => servicesMap[p.ParameterType]).ToArray();

        return ctor.Invoke(parameters);
    }
    
    public static IEnumerable<Type> GetInterfaceSubclassTypes<T>(Assembly assembly) where T : class => 
        assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(T)) && t.IsClass && !t.IsAbstract && t.IsVisible);

    public static IEnumerable<object> CreateTypesInstances(IEnumerable<Type> types) => 
        types.Select(Activator.CreateInstance).Where(x => x != null)!;
    
    public static IEnumerable<T> CreateTypesInstances<T>(IEnumerable<Type> types) => 
        types.Where(t => t.IsAssignableTo(typeof(T))).Select(Activator.CreateInstance).Where(x => x != null).Select(x => (T)x!);
}