namespace YumeScript.Exceptions.Parser;

public class ParserException : Exception
{
    public string ScriptFullName;
    public int Line;
    
    public ParserException(string scriptFullName, int line, Exception? innerException = null) : 
        base($"Failed to parse line {line} file {scriptFullName}", innerException)
    {
        ScriptFullName = scriptFullName;
        Line = line;
    }
    
    public ParserException(Exception? innerException = null) : 
        base($"Failed to parse line", innerException)
    {
        ScriptFullName = string.Empty;
        Line = -1;
    }
}