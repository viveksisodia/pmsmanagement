namespace Construction.Domain;

/// <summary>A business rule was broken. The message is shown to the user.</summary>
public sealed class DomainException(string message) : Exception(message);
