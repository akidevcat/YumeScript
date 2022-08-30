using YumeScript.Script;

namespace YumeScript.Parser;

public interface IInstructionParser // ToDo Per-instruction stack instead of singletons?
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
    IEnumerable<RuntimeInstruction>? ParseLineTokens(string[] tokens);

    /// <summary>
    /// Intercepts normal line parsing workflow after indention calculation.
    /// </summary>
    /// <param name="tokens">Line tokens</param>
    /// <returns>
    /// State, whether the parsing workflow should continue or not, and an instruction
    /// that should be added to the resulted script.
    /// </returns>
    IEnumerable<RuntimeInstruction>? InterceptLineTokens(string[] tokens);

    IEnumerable<RuntimeInstruction> FinalizeIndentionSection();
}