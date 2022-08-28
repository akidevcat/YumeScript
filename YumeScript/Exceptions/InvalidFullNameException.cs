namespace YumeScript.Exceptions;

public class InvalidFullNameException : Exception
{
    public InvalidFullNameException() : base("Received an invalid full name")
    {
        
    }
    
    public InvalidFullNameException(string fullName) : base($"Received invalid full name: {fullName}")
    {
        
    }
}