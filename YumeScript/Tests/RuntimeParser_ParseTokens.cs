using NUnit.Framework;
using YumeScript.Parser;

namespace YumeScript.Tests;

[TestFixture]
public class RuntimeParser_ParseTokens
{
    [Test]
    public void Run()
    {
        var result = ScriptParser.ParseTokens(
            "\t\t\t\t* \"After waiting for what felt like half an hour, a girl entered the room.\" # Test Comment");
        
        Assert.AreEqual(4, result.Item1);
        Assert.AreEqual(2, result.Item2.Length);
        Assert.AreEqual("*", result.Item2[0]);
        Assert.AreEqual("\"After waiting for what felt like half an hour, a girl entered the room.\"", result.Item2[1]);
    }
}