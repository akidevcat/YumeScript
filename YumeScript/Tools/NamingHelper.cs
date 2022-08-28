using System.Text.RegularExpressions;

namespace YumeScript.Tools;

public static class NamingHelper
{
    private static readonly Regex FullNameRegex = new(@"^([A-z]+\.)*[A-z]+$");

    public static bool IsFullNameValid(string fullName) => FullNameRegex.Match(fullName).Success;
}