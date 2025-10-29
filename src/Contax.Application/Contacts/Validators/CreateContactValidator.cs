using Contax.Application.Contacts.Commands;
using FluentValidation;

namespace Contax.Application.Contacts.Validators;

public class CreateContactValidator : AbstractValidator<CreateContactCommand>
{
    public CreateContactValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("O nome é obrigatório.").MinimumLength(3);
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("O email é obrigatório.")
            .When(x => !string.IsNullOrEmpty(x.Email));
        RuleFor(x => x.Phone)
            .NotEmpty()
            .Matches(@"^\+?\d{8,15}$")
            .WithMessage(
                "O telefone é obrigatório e deve conter entre 8 a 15 dígitos, podendo iniciar com '+'."
            );
    }
}
