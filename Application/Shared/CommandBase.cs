using Domain.Interfaces;
using FluentValidation.Results;

namespace Application.Shared;

public class CommandBase : ICommandBase
{
    public bool Valid => Errors.Count == 0;

    public List<KeyValuePair<string, string>> Errors { get; private set; } = [];


    public virtual void Validate()
    {
        throw new NotImplementedException();
    }

    public void AddErros(ValidationResult validationResult)
    {
        Errors = [];
        foreach (ValidationFailure error in validationResult.Errors)
        {
            Errors.Add(new KeyValuePair<string, string>(error.PropertyName, error.ErrorMessage));
        }
    }
}