namespace YumeScript.Translator.External;

public interface IScriptParser
{
    /// <summary>
    /// Returns parser's priority.
    /// Lower the number, earlier the parser will be invoked.
    /// </summary>
    /// <returns>Parser's priority.</returns>
    int GetPriority();
    
    /// <summary>
    /// Parses line tokens into an instruction if applicable
    /// </summary>
    /// <param name="tokens">Line tokens</param>
    /// <returns>An instruction if successful, else null</returns>
    ParserResult ParseLineTokens(int instructionId, string[] tokens);

    /// <summary>
    /// Intercepts normal line parsing workflow after indention calculation.
    /// </summary>
    /// <param name="tokens">Line tokens</param>
    /// <returns>
    /// State, whether the parsing workflow should continue or not, and an instruction
    /// that should be added to the resulted script.
    /// </returns>
    ParserResult InterceptLineTokens(int instructionId, string[] tokens, int relativeIndentionLevel);

    /// <summary>
    /// Finalizes (before dereferencing and being removed) indention section.
    /// </summary>
    /// <returns>
    /// bool - whether this parser should be kept for the following indention section
    /// 
    /// </returns>
    FinalizationParserResult FinalizeIndentionSection(int instructionId, string[] tokens);
}