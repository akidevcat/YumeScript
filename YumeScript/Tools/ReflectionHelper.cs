using System.Reflection;

namespace YumeScript.Tools;

public static class ReflectionHelper
{
    public static IEnumerable<Type> GetInterfaceSubclassTypes<T>(Assembly assembly) where T : class => 
        assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(T)) && t.IsClass && !t.IsAbstract && t.IsVisible);

    public static IEnumerable<object> CreateTypesInstances(IEnumerable<Type> types) => 
        types.Select(Activator.CreateInstance).Where(x => x != null)!;
    
    public static IEnumerable<T> CreateTypesInstances<T>(IEnumerable<Type> types) => 
        types.Where(t => t.IsAssignableTo(typeof(T))).Select(Activator.CreateInstance).Where(x => x != null).Select(x => (T)x!);
}