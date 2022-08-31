using YumeScript.Exceptions.Parser;
using YumeScript.External;
using YumeScript.Script;

namespace YumeScript.Parser;

public class ScriptTree : IScriptTree
{
    private RuntimeFunctionBuilder? _appendingFunction;
    private readonly List<RuntimeFunction> _functions;
    private readonly HashSet<string> _functionNames;
    
    public RuntimeInstruction? this[int lineId]
    {
        get
        {
            if (lineId < 0)
            {
                throw new IndexOutOfRangeException();
            }
            
            var funcPtr = 0;
            foreach (var func in _functions)
            {
                var len = func.Instructions.Length;

                if (lineId < funcPtr + len)
                {
                    return func.Instructions[lineId - funcPtr];
                }

                funcPtr += len;
            }

            if (_appendingFunction == null)
            {
                return null;
            }
            
            if (lineId < funcPtr + _appendingFunction.Count)
            {
                return _appendingFunction[lineId - funcPtr];
            }

            return null;
        }
        set
        {
            if (value == null)
            {
                throw new NullReferenceException();
            }

            if (lineId < 0)
            {
                throw new IndexOutOfRangeException();
            }
            
            var funcPtr = 0;
            foreach (var func in _functions)
            {
                var len = func.Instructions.Length;

                if (lineId < funcPtr + len)
                {
                    func.Instructions[lineId - funcPtr] = value.Value;
                }

                funcPtr += len;
            }

            if (_appendingFunction == null)
            {
                throw new IndexOutOfRangeException();
            }
            
            if (lineId < funcPtr + _appendingFunction.Count)
            {
                _appendingFunction[lineId - funcPtr] = value.Value;
            }

            throw new IndexOutOfRangeException();
        }
    }

    public int Length
    {
        get
        {
            var result = _functions.Sum(func => func.Instructions.Length);
            
            result += _appendingFunction?.Count ?? 0;
            
            return result;
        }
    }
    
    internal ScriptTree()
    {
        _functions = new List<RuntimeFunction>();
        _functionNames = new HashSet<string>();
    }

    internal bool AppendingFunction => _appendingFunction != null;
    
    internal void CreateAppendingFunction(string name)
    {
        if (ContainsFunction(name))
            throw new FunctionNameExistsException();

        _appendingFunction = new RuntimeFunctionBuilder(name);
        _functionNames.Add(name);
    }
    
    internal bool TryBuildFunction()
    {
        if (_appendingFunction == null)
        {
            return false;
        }
        
        BuildFunction();
        return true;
    }
    
    internal void BuildFunction()
    {
        if (_appendingFunction == null)
        {
            throw new NullReferenceException(); // ToDo
        }
        
        _functions.Add(_appendingFunction.Build());
        _appendingFunction = null;
    }

    internal bool ContainsFunction(string name) => _functionNames.Contains(name);

    internal void AppendInstructions(IEnumerable<RuntimeInstruction> instructions)
    {
        if (_appendingFunction == null)
        {
            throw new NullReferenceException(); // ToDo
        }

        _appendingFunction.AddRange(instructions);
    }

    internal Dictionary<string, RuntimeFunction> BuildTree()
    {
        TryBuildFunction();
        return _functions.ToDictionary(x => x.Name);
    }
}