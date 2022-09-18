namespace YumeScript.External;

public interface ICallbackEngine
{
    /// <summary>
    /// Makes a statement call.
    /// Might be used to evaluate conditions.
    /// </summary>
    /// <param name="source">Source code of the call.</param>
    /// <returns>Returned object</returns>
    Task<object?> CallStatement(string source);

    /// <summary>
    /// Prints a phrase spoken by a character
    /// </summary>
    /// <param name="charName">Character name</param>
    /// <param name="charText">Character text</param>
    /// <param name="args">Call arguments</param>
    /// <returns></returns>
    Task PrintPhrase(string charName, string charText, string[] args);

    /// <summary>
    /// Awaits for player to choice branching path
    /// </summary>
    /// <param name="options">Options available to player</param>
    /// <param name="args">Call arguments</param>
    /// <returns>Index of the chosen path</returns>
    Task<int> AwaitBranching(string[] options, string[] args);
    
    // ToDo variables
}