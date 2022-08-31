using YumeScript.External;
using YumeScript.Parser;
using YumeScript.Tools;

namespace YumeScript.Script;

// ToDo: Make unmanaged
public struct RuntimeInstruction
{
    public int Type;
    public string MainData;
    public string[] Arguments;

    public RuntimeInstruction(Type? type, string mainData, string[]? arguments = null)
    {
        Type = type != null ? NamingHelper.GetObjectTypeHashcode(type) : 0;
        MainData = mainData;
        Arguments = arguments ?? Array.Empty<string>();
    }
}