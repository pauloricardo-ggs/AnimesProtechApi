using FluentValidation;

namespace Application.Shared.Base;

public class ValidatorBase<T> : AbstractValidator<T>
{
    public ValidatorBase()
    {
        // Se quiser mensagens em português
        // ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("pt-BR"); 
    }
}