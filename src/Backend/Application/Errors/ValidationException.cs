using ApplicationException = Domain.Exceptions.ApplicationException;

namespace Application.Errors;

public sealed class ValidationException : ApplicationException
{
    public ValidationException(IReadOnlyDictionary<string, string[]> errorsDictionary) :
        base("Validation Failure", "One or more validation errors ocurred") => ErrorsDictionary = errorsDictionary;

    public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; }
}
