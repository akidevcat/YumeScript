using System.Text.RegularExpressions;

namespace YumeScript.SDK.Tools;

public static class NamingHelper
{
    private static readonly Regex FullNameRegex = new(@"^([A-z]+\.)*[A-z]+$");
    private static readonly Regex FunctionNameRegex = new(@"^[a-zA-Z_]+[a-zA-Z_0-9]*");

    public static bool IsFullNameValid(string name) => FullNameRegex.Match(name).Success;

    public static bool IsFunctionNameValid(string name) => FunctionNameRegex.Match(name).Success;

    public static int GetTypeHashcode(Type type) => (type.FullName ?? type.Name).GetHashCode();
    
    public static int GetObjectTypeHashcode(object obj) => string.GetHashCode(obj.GetType().FullName ?? obj.GetType().Name);
}