namespace YumeScript.Translator.Exceptions;

public class TranslationException : Exception
{
    public string ScriptFullName;
    public int Line;
    
    public TranslationException(string scriptFullName, int line, Exception? innerException = null) : 
        base($"Failed to parse line {line} file {scriptFullName}", innerException)
    {
        ScriptFullName = scriptFullName;
        Line = line;
    }
    
    public TranslationException(Exception? innerException = null) : 
        base($"Failed to parse line", innerException)
    {
        ScriptFullName = string.Empty;
        Line = -1;
    }
}