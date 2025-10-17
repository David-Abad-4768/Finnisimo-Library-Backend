namespace Finnisimo_Library_Backend.Application.Exceptions;

public sealed record ValidationError(string PropertyName, string ErrorMessage);
