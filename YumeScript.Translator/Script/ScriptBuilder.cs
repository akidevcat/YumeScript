using System.Collections.Immutable;
using YumeScript.Parser;
using YumeScript.Translator.External;

namespace YumeScript.Translator.Script;

public class ScriptBuilder : IScriptBuilder
{
    internal readonly object AccessObject;
    private ScriptFunctionBuilder? _appendingFunction;
    private ScriptFunctionBuilder? _appendingFunctionCopy;
    private bool _appendingFunctionChanged = false;
    private readonly List<ScriptFunction> _functions;
    private readonly HashSet<string> _functionNames;
    private Dictionary<int, object> _constants;
    private Dictionary<int, object>? _constantsCopy;
    private bool _constantsChanged = false;
    
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
                return _appendingFunction[lineId - funcPtr].Item2;
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
                var temp = _appendingFunction[lineId - funcPtr];
                temp.Item2 = value.Value;
                _appendingFunction[lineId - funcPtr] = temp;
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

    internal ScriptBuilder()
    {
        AccessObject = new();
        _functions = new ();
        _functionNames = new();
        _constants = new();
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
        
        Append(new ScriptInstruction(typeof(ReturnEvaluator)), AccessObject);
        _functions.Add(_appendingFunction.Build());
        _appendingFunction = null;
    }

    internal bool ContainsFunction(string name) => _functionNames.Contains(name);

    public int Append(IEnumerable<ScriptInstruction> instructions, object? caller = null)
    {
        if (_appendingFunction == null)
        {
            throw new NullReferenceException(); // ToDo
        }

        var result = Length;
        _appendingFunction.AddRange(instructions.Select(x => (caller, x)));
        _appendingFunctionChanged = true;
        return result;
    }

    public int Append(ScriptInstruction instruction, object? caller = null)
    {
        if (_appendingFunction == null)
        {
            throw new NullReferenceException(); // ToDo
        }
        
        var result = Length;
        _appendingFunction.Add((caller, instruction));
        _appendingFunctionChanged = true;
        return result;
    }

    public int Insert(IEnumerable<ScriptInstruction> instructions, int startAt, object? caller = null)
    {
        if (_appendingFunction == null)
        {
            throw new NullReferenceException(); // ToDo
        }

        if (startAt < _appendingFunction.Pointer)
        {
            throw new ArgumentOutOfRangeException(nameof(startAt), "Instruction index was out of appending function range");
        }

        var subIndex = startAt - _appendingFunction.Pointer;
        _appendingFunction.InsertRange(subIndex, instructions.Select(x => (caller, x)));
        _appendingFunctionChanged = true;
        return startAt;
    }
    
    public int Insert(ScriptInstruction instruction, int startAt, object? caller = null)
    {
        if (_appendingFunction == null)
        {
            throw new NullReferenceException(); // ToDo
        }

        if (startAt < _appendingFunction.Pointer)
        {
            throw new ArgumentOutOfRangeException(nameof(startAt), "Instruction index was out of appending function range");
        }

        var subIndex = startAt - _appendingFunction.Pointer;
        _appendingFunction.Insert(subIndex, (caller, instruction));
        _appendingFunctionChanged = true;
        return startAt;
    }

    public void Update(int index, Func<ScriptInstruction, ScriptInstruction> updateFunc, object? caller = null)
    {
        if (_appendingFunction == null)
        {
            throw new NullReferenceException(); // ToDo
        }

        if (index < _appendingFunction.Pointer)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Instruction index was out of appending function range");
        }

        var subIndex = index - _appendingFunction.Pointer;
        var value = _appendingFunction[subIndex];

        // Check access
        if (caller != AccessObject && caller != value.Item1)
        {
            throw new AccessViolationException("Specified caller has no access to the given instruction index");
        }

        value.Item2 = updateFunc(value.Item2);
        _appendingFunctionChanged = true;
    }

    public int Allocate(object constant)
    {
        if (!constant.GetType().IsSerializable)
        {
            throw new ArgumentException("Argument is not serializable", nameof(constant));
        }
        
        var hash = constant.GetHashCode();
        if (!_constants.ContainsKey(hash))
        {
            _constants.Add(hash, constant);
            _constantsChanged = true;
        }

        return hash;
    }

    internal void SaveCurrentState()
    {
        _appendingFunctionCopy = (ScriptFunctionBuilder)_appendingFunction!.Clone();
        _constantsCopy = _constants.ToDictionary(e => e.Key, e => e.Value.Copy());
        _appendingFunctionChanged = false;
        _constantsChanged = false;
    }

    internal void LoadSavedState()
    {
        _appendingFunction = _appendingFunctionCopy;
        _constants = _constantsCopy!;
        _appendingFunctionCopy = null;
        _constantsCopy = null;
    }

    internal ImmutableDictionary<string, ScriptFunction> BuildTree()
    {
        TryBuildFunction();
        return _functions.ToImmutableDictionary(x => x.Name);
    }
    
    internal ImmutableDictionary<int, object> BuildConstants()
    {
        return _constants.ToImmutableDictionary();
    }
}