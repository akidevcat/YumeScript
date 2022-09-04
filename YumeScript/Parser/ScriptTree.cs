using System.Collections.Immutable;
using YumeScript.Exceptions.Parser;
using YumeScript.External;
using YumeScript.Runtime.InstructionEvaluators;
using YumeScript.Script;

namespace YumeScript.Parser;

public class ScriptTree : IScriptTree
{
    private ScriptFunctionBuilder? _appendingFunction;
    private readonly List<ScriptFunction> _functions;
    private readonly HashSet<string> _functionNames;
    private readonly Dictionary<int, IConvertible> _constants;
    
    public ScriptInstruction? this[int lineId] // ToDo
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
                return;
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
        _functions = new ();
        _functionNames = new();
        _constants = new();
        
        _constants.Add(int.MaxValue, "Registry Value A");
        _constants.Add(int.MaxValue - 1, "Registry Value B");
        _constants.Add(int.MaxValue - 2, "Registry Value C");
        _constants.Add(int.MaxValue - 3, "Registry Value D");
    }

    internal bool AppendingFunction => _appendingFunction != null;

    internal void CreateAppendingFunction(string name)
    {
        if (ContainsFunction(name))
            throw new FunctionNameExistsException();

        if (_appendingFunction != null)
            throw new Exception("Appending function already exists"); // ToDo
        
        _appendingFunction = new ScriptFunctionBuilder(name, Length);
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
        
        _appendingFunction.Add(new ScriptInstruction(typeof(ReturnEvaluator))); // ToDo change to external call
        _functions.Add(_appendingFunction.Build());
        _appendingFunction = null;
    }

    internal bool ContainsFunction(string name) => _functionNames.Contains(name);

    internal void AppendInstructions(IEnumerable<ScriptInstruction> instructions)
    {
        if (_appendingFunction == null)
        {
            throw new NullReferenceException(); // ToDo
        }

        _appendingFunction.AddRange(instructions);
    }
    
    public int Allocate(IConvertible constant)
    {
        var hash = constant.GetHashCode();
        if (!_constants.ContainsKey(hash))
            _constants.Add(hash, constant);
        return hash;
    }

    internal ImmutableDictionary<string, ScriptFunction> BuildTree()
    {
        TryBuildFunction();
        return _functions.ToImmutableDictionary(x => x.Name);
    }
    
    internal ImmutableDictionary<int, IConvertible> BuildConstants()
    {
        return _constants.ToImmutableDictionary();
    }
}