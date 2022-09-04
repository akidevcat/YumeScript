using System.Collections.Immutable;
using System.Globalization;
using YumeScript.Runtime;
using YumeScript.Script;

namespace YumeScript.Tools;

public static class DebugHelper
{
    public static void PrintScriptInstructions(RuntimeInstance runtime, Script.Script script)
    {
        string OpToString(int op)
        {
            return script.OpConstants!.ContainsKey(op) ? "0x" + op.ToString("X8") : op.ToString();
        }
        
        Console.WriteLine($"Script {script.FullName}");
        
        if (!script.IsParsed)
        {
            Console.WriteLine("not parsed.");
            return;
        }
        
        Console.WriteLine("Functions:");
        var functions = script.Functions!.Values.OrderBy(f => f.Pointer);
        foreach (var func in functions)
        {
            Console.WriteLine($"\t{func.Pointer} {func.Name}:");
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
                var right = $"{OpToString(i.OpA)} {OpToString(i.OpB)} {OpToString(i.OpC)} {OpToString(i.OpD)}";
                const int offset = 80;

                Console.WriteLine("\t\t" + left + right.PadLeft(offset - left.Length));
            }
        }
        
        Console.WriteLine("Constants:");
        foreach (var (cHash, cValue) in script.OpConstants!)
        {
            var left = $"0x{cHash:X8}";
            var right = cValue.ToString(CultureInfo.InvariantCulture).Replace("\n", "\\n");
            const int offset = 80;

            Console.WriteLine("\t" + left + "\t" + right.PadLeft(offset - left.Length));
        }
    }
}