namespace YumeScript.Translator.Exceptions;

public class IndentionException : Exception
{
    public int ExpectedIndention;
    public int ActualIndention;
    public string ScriptFullName;
    public int Line;
    
    public IndentionException(int expectedIndention, int actualIndention, string scriptFullName, int line, Exception? innerException = null) : 
        base($"Expected indention level {expectedIndention} instead of {actualIndention} on line {line + 1} file '{scriptFullName}'", innerException)
    {
        ExpectedIndention = expectedIndention;
        ActualIndention = actualIndention;
        ScriptFullName = scriptFullName;
        Line = line;
    }
    
    public IndentionException(int expectedIndention, int actualIndention, Exception? innerException = null) : 
        base($"Expected indention level {expectedIndention} instead of {actualIndention}", innerException)
    {
        ExpectedIndention = expectedIndention;
        ActualIndention = actualIndention;
        ScriptFullName = string.Empty;
        Line = -1;
    }
}