using YumeScript.Parser;
using YumeScript.Tools;

namespace YumeScript.Script;

// ToDo: Make unmanaged
public struct RuntimeInstruction
{
    public int Type;
    public string MainData;
    public string[] Arguments;

    public RuntimeInstruction(IInstructionParser parser, string mainData, string[]? arguments = null)
    {
        Type = NamingHelper.GetObjectTypeHashcode(parser);
        MainData = mainData;
        Arguments = arguments ?? Array.Empty<string>();
    }
}