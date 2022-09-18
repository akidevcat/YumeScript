using System.Text;

namespace YumeScript.SDK.Script;

[Serializable]
public struct ScriptInstruction
{
    public int EncodedCode;
    public int OpA;
    public int OpB;
    public int OpC;
    public int OpD;

    public ScriptInstruction(string code, int opA = 0, int opB = 0, int opC = 0, int opD = 0)
    {
        if (code.Length != 4)
        {
            throw new ArgumentException("Invalid code length, expected 4 chars", nameof(code));
        }

        EncodedCode = BitConverter.ToInt32(Encoding.ASCII.GetBytes(code), 0);
        OpA = opA;
        OpB = opB;
        OpC = opC;
        OpD = opD;
    }
    
    public ScriptInstruction(int encodedCode, int opA = 0, int opB = 0, int opC = 0, int opD = 0)
    {
        EncodedCode = encodedCode;
        OpA = opA;
        OpB = opB;
        OpC = opC;
        OpD = opD;
    }
}