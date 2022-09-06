using System.Text;
using System.Text.RegularExpressions;
using YumeScript.Configuration;
using YumeScript.Exceptions.Parser;
using YumeScript.External;
using YumeScript.Runtime;
using YumeScript.Script;
using YumeScript.Tools;

namespace YumeScript.Parser;

internal static class ScriptParser
{
    internal static void ParseScript(this RuntimeInstance runtime, Script.Script script)
    {
        if (script.IsParsed)
            return;

        if (script.SourceCode == null)
            return;

        var scriptTree = new ScriptTree();
        var currentIndentionLevel = 0;

        //RuntimeFunctionBuilder? currentFunction = null;
        var parserStack = new Stack<(int, IInstructionParser)>(); // Indention level, parser

        for (var i = 0; i < script.SourceCode.Count; i++)
        {
            try
            {
                var line = script.SourceCode[i];
                var (tabsCount, tokens) = ParseTokens(line);
                
                // Skip empty lines
                if (tokens.Length == 0)
                    continue;

                var indentionDelta = tabsCount - currentIndentionLevel;

                // Check for incorrect indention increase
                if (indentionDelta > 0)
                    throw new IndentionException(currentIndentionLevel, tabsCount, script.FullName, i);

                var flSkipFurtherParsing = false;
                
                // Check for indention level decrease - pop stack
                while (parserStack.Count > 0 && tabsCount <= parserStack.Peek().Item1)
                {
                    if (!scriptTree.AppendingFunction)
                    {
                        throw new NullReferenceException();
                    }

                    // Pass tokens only if frame has the same indention level
                    var parserResult = parserStack.Peek().Item2.FinalizeIndentionSection(scriptTree.Length, tabsCount == parserStack.Peek().Item1 ? tokens : Array.Empty<string>());

                    if (parserResult.Instructions == null)
                    {
                        throw new NullReferenceException(); // ToDo
                    }

                    // Parser can be kept only if this is a paired instruction
                    if (parserResult.KeepParser && !tokens[^1].EndsWith(":"))
                    {
                        throw new ParserException(); //ToDo
                    }
                    
                    scriptTree.AppendInstructions(parserResult.Instructions);

                    if (!parserResult.KeepParser || tabsCount != parserStack.Peek().Item1)
                    {
                        parserStack.Pop();
                    }
                    else
                    {
                        flSkipFurtherParsing = true;
                        break;
                    }
                }
                
                if (flSkipFurtherParsing)
                    continue;
                
                // Check for zero indention - function closure
                if (indentionDelta < 0 && tabsCount == 0)
                {
                    scriptTree.BuildFunction();
                }

                currentIndentionLevel = tabsCount;

                // Check for function declaration
                if (!scriptTree.AppendingFunction)
                {
                    var functionName = ParseFunctionDeclaration(tabsCount, tokens);
                    
                    if (functionName == null)
                        continue;

                    scriptTree.CreateAppendingFunction(functionName);
                    currentIndentionLevel = 1;

                    continue;
                }
                
                // --- currentFunction is not null ---

                IEnumerable<ScriptInstruction>? instructionsToAppend = null;
                IInstructionParser? currentParser = null;

                // Send current line to active parser interception
                if (parserStack.Count > 0)
                {
                    var (activeParserIndention, activeParser) = parserStack.Peek();
                    instructionsToAppend = activeParser.InterceptLineTokens(scriptTree.Length, tokens).Instructions;

                    if (instructionsToAppend != null)
                    {
                        currentParser = activeParser;
                    }
                }

                // If active parser skipped this line - use instruction parsers
                if (instructionsToAppend == null)
                {
                    foreach (var (priority, parserType) in runtime.InstructionParsers)
                    {
                        var parser = (IInstructionParser)ReflectionHelper
                            .CreateInjectedInstance(parserType, (typeof(IScriptTree), scriptTree));
                        
                        instructionsToAppend = parser.ParseLineTokens(scriptTree.Length, tokens).Instructions;

                        if (instructionsToAppend != null)
                        {
                            currentParser = parser;
                            break;
                        }
                    }
                }
                
                if (currentParser == null)
                {
                    throw new UnknownInstructionException();
                }
                
                // Check if instruction requires indention elevation
                if (tokens[^1].EndsWith(":"))
                {
                    parserStack.Push((currentIndentionLevel, currentParser));
                    currentIndentionLevel++;
                }
                
                // Add instructions to current function
                if (instructionsToAppend != null)
                {
                    scriptTree.AppendInstructions(instructionsToAppend);
                }
            }
            catch (TokenExpectedException ex)
            {
                throw new TokenExpectedException(ex.TokenName, script.FullName, i, ex.Position, ex);
            }
            catch (IndentionException ex)
            {
                throw new IndentionException(ex.ExpectedIndention, ex.ActualIndention, script.FullName, i, ex);
            }
        }
        
        // Clear remaining stack
        while (parserStack.TryPop(out var parserFrame))
        {
            if (!scriptTree.AppendingFunction)
            {
                throw new NullReferenceException();
            }
            
            var parserResult = parserFrame.Item2.FinalizeIndentionSection(scriptTree.Length, Array.Empty<string>());
            
            if (parserResult.Instructions == null)
            {
                throw new NullReferenceException(); // ToDo
            }
            
            scriptTree.AppendInstructions(parserResult.Instructions);
        }

        script.Functions = scriptTree.BuildTree();
        script.OpConstants = scriptTree.BuildConstants();
    }

    /// <summary>
    /// Parses line tokens as a function
    /// </summary>
    /// <param name="tabsCount"></param>
    /// <param name="tokens"></param>
    /// <returns>null if no function declaration found</returns>
    /// <exception cref="IndentionException"></exception>
    internal static string? ParseFunctionDeclaration(int tabsCount, string[] tokens)
    {
        // Skip empty lines
        if (tokens.Length == 0)
            return null;
        
        // Skip invalid syntax
        if (tokens.Length > 1)
            throw new FunctionExpectedException();
        
        // Check for invalid indention
        if (tabsCount > 0)
            throw new IndentionException(0, tabsCount);

        var name = tokens[0];
        
        // Check for : token
        if (!name.EndsWith(':'))
            throw new TokenExpectedException(":", tabsCount + name.Length - 1);

        name = name[..^1];
        
        // Validate function name
        if (name.Length == 0)
            throw new FunctionExpectedException();

        if (!NamingHelper.IsFunctionNameValid(name))
            throw new InvalidFunctionNameException();

        return name;
    }
    
    internal static ScriptInstruction? ParseInstruction(string[] tokens)
    {
        if (tokens.Length == 0)
            return null;

        return null;
    }

    /// <summary>
    /// Parses a line of code into tokens - elements between spaces - taking into account string quotes
    /// </summary>
    /// <param name="line">Line that should be parsed</param>
    /// <returns>Tabs count at the beginning, Parsed tokens</returns>
    internal static (int, string[]) ParseTokens(string line)
    {
        var result = new List<string>();
        var currentToken = new StringBuilder();
        var tabsPrefix = 0;

        // Is inside double quotes?
        var flDoubleQuote = false;
        // Is inside single quotes?
        var flSingleQuote = false;
        // Is counting tabs prefix?
        var flTabsPrefix = true;

        void PushCurrentToken()
        {
            if (currentToken.Length <= 0) return;

            result.Add(currentToken.ToString());
            currentToken.Clear();
        }

        for (var i = 0; i < line.Length; i++)
        {
            // Previous char
            var p = i > 0 ? line[i - 1] : '\0';
            // Current char
            var c = line[i];
            // Skip for any char following after \
            if ((flDoubleQuote || flSingleQuote) && p == '\\')
            {
                currentToken.Append(c);
                continue;
            }

            // Escape quotes
            if (c == '"' && (flDoubleQuote || (!flSingleQuote && !flDoubleQuote)))
            {
                flDoubleQuote = !flDoubleQuote;
            }
            if (c == '\'' && (flSingleQuote || (!flSingleQuote && !flDoubleQuote)))
            {
                flSingleQuote = !flSingleQuote;
            }

            // Skip everything as a comment
            if (!flSingleQuote && !flDoubleQuote && c == Constants.SyntaxCommentSymbol)
                break;

            // Count tabs
            if (flTabsPrefix)
            {
                if (c == '\t')
                {
                    tabsPrefix += 4;
                    continue;
                }
                else if (c == ' ')
                {
                    tabsPrefix += 1;
                    continue;
                }
                else
                {
                    flTabsPrefix = false;
                }
            }

            // Push current token
            if (!flSingleQuote && !flDoubleQuote && c == ' ')
            {
                PushCurrentToken();
                continue;
            }

            currentToken.Append(c);
        }

        PushCurrentToken();

        if (flSingleQuote)
            throw new TokenExpectedException("'", line.Length - 1);

        if (flDoubleQuote)
            throw new TokenExpectedException("\"", line.Length - 1);
        
        return (tabsPrefix / 4, result.ToArray());
    }
}