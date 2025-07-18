/// <summary>
/// Exception thrown when Rejigs validation fails.
/// </summary>
public class RejigsValidationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the RejigsValidationException class.
    /// </summary>
    /// <param name="message">The error message</param>
    public RejigsValidationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the RejigsValidationException class.
    /// </summary>
    /// <param name="message">The error message</param>
    /// <param name="innerException">The inner exception</param>
    public RejigsValidationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}