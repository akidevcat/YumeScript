namespace YumeScript;

public interface ICallbackEngine
{
    object? CallStatement(string source);
}