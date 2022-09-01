namespace YumeScript.Runtime;

public interface IRuntimeExecution
{
    int ReadIntValue(string name);
    double ReadDoubleValue(string name);
    string ReadStringValue(string name);
}