namespace YumeScript.Runtime;

internal class RuntimeThread
{
    public object? CodeExecutionRegistry;
    public readonly Stack<ExecutionFrame> ExecutionStack;

    public RuntimeThread()
    {
        ExecutionStack = new Stack<ExecutionFrame>();
    }
}