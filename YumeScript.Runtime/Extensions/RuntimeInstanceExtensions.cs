using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using YumeScript.Configuration;
using YumeScript.Runtime;

namespace YumeScript.Extensions;

public static class RuntimeInstanceExtensions
{
    public static string ConvertScriptToString(this RuntimeInstance runtime, Script.Script script)
    {
        var result = new StringBuilder();
        
        var temp = 0;
        var constantsDict = script.OpConstants!.ToImmutableDictionary(k => k.Key, v => (v.Value, temp++));
        
        string OpToString(int op)
        {
            return script.OpConstants!.ContainsKey(op) ? "0x" + op.ToString("X8") : op.ToString();
        }
        
        string OpToStringDict(int op)
        {
            if (op == Constants.RegistryAddressResult)
                return "res";
            return constantsDict.ContainsKey(op) ? "^" + constantsDict[op].Item2 : op.ToString();
        }
        
        result.AppendLine($"Script {script.FullName}");
        
        if (!script.IsParsed)
        {
            return "Script is not parsed.";
        }

        result.AppendLine("Functions:");
        var functions = script.Functions!.Values.OrderBy(f => f.Pointer);
        foreach (var func in functions)
        {
            result.AppendLine($"\t{func.Pointer} {func.Name}:");
            for (var index = 0; index < func.Instructions.Length; index++)
            {
                var i = func.Instructions[index];
                var typeName = "null";
                var typeId = i.Type;
                if (runtime.InstructionEvaluators.ContainsKey(typeId))
                {
                    typeName = runtime.InstructionEvaluators[typeId].GetType().Name;
                }

                if (typeName.EndsWith("Evaluator"))
                {
                    typeName = typeName.Remove(typeName.Length - "Evaluator".Length);
                }

                var left = $"{func.Pointer + index} {typeName}";
                //var right = $"{OpToString(i.OpA)} {OpToString(i.OpB)}";
                var right = $"{OpToStringDict(i.OpA)},{OpToStringDict(i.OpB)},{OpToStringDict(i.OpC)},{OpToStringDict(i.OpD)}";
                const int offset = 80;

                result.AppendLine("\t\t" + left + right.PadLeft(offset - left.Length));
            }
        }
        
        result.AppendLine("Constants:");
        foreach (var (cHash, cValue) in constantsDict!)
        {
            var cValueString = ObjectToString(cValue.Value);
            
            //var left = $"0x{cHash:X8}";
            var left = $"{cValue.Item2}";
            var right = cValueString.Replace("\n", "\\n");
            const int offset = 80;

            result.AppendLine("\t" + left + "\t" + right.PadLeft(offset - left.Length));
        }
        
        result.AppendLine("Registers:");
        result.AppendLine($"\t^res - Result Registry");

        return result.ToString();
    }

    private static string ObjectToString(object obj)
    {
        return obj.GetType().Name switch
        {
            "String[]" => $"[\"{string.Join("\", \"", (string[])obj)}\"]",
            "String" => $"\"{obj}\"",
            _ => $"{obj}"
        };
    }
}