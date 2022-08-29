using System.Text;
using System.Text.RegularExpressions;
using YumeScript.Configuration;
using YumeScript.Exceptions.Parser;
using YumeScript.Script;

namespace YumeScript.Parser;

internal static class RuntimeParser
{
    private static readonly Regex CommentsRegex = new(@"^([A-z]+\.)*[A-z]+$");
    
    internal static void ParseScript(RuntimeScript script)
    {
        if (script.IsParsed)
        {
            return;
        }

        if (script.SourceCode == null)
        {
            return;
        }

        for (var i = 0; i < script.SourceCode.Count; i++)
        {
            try
            {
                var line = script.SourceCode[i];
                var (tabsCount, tokens) = ParseTokens(line);

                
            }
            catch (TokenExpectedException ex)
            {
                throw new TokenExpectedException(ex.TokenName, script.FullName, i, ex.Position, ex);
            }
        }
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
            {
                break;
            }

            // Count tabs
            if (flTabsPrefix)
            {
                if (c == '\t')
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
        {
            throw new TokenExpectedException("'", line.Length - 1);
        }

        if (flDoubleQuote)
        {
            throw new TokenExpectedException("\"", line.Length - 1);
        }
        
        return (tabsPrefix, result.ToArray());
    }
}