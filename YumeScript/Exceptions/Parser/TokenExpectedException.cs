namespace YumeScript.Exceptions.Parser;

public class TokenExpectedException : Exception
{
    public string TokenName;
    public string ScriptFullName;
    public int Line;
    public int Position;
    
    public TokenExpectedException(string tokenName, string scriptFullName, int line, int position, Exception? innerException = null) : 
        base($"Expected '{tokenName}' on line {line + 1} at {position} file '{scriptFullName}'", innerException)
    {
        TokenName = tokenName;
        ScriptFullName = scriptFullName;
        Line = line;
        Position = position;
    }
    
    public TokenExpectedException(string tokenName, int position, Exception? innerException = null) : 
        base($"Expected '{tokenName}' at {position}", innerException)
    {
        TokenName = tokenName;
        ScriptFullName = string.Empty;
        Position = position;
        Line = -1;
    }
}