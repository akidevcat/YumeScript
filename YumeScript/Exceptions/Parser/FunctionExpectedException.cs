namespace YumeScript.Exceptions.Parser;

public class FunctionExpectedException : Exception
{
    public string ScriptFullName;
    public int Line;
    
    public FunctionExpectedException(string scriptFullName, int line, Exception? innerException = null) : 
        base($"Expected function declaration on line {line + 1} file '{scriptFullName}'", innerException)
    {
        ScriptFullName = scriptFullName;
        Line = line;
    }
    
    public FunctionExpectedException(Exception? innerException = null) : 
        base($"Expected function declaration", innerException)
    {
        ScriptFullName = string.Empty;
        Line = -1;
    }
}