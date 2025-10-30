using Contax.Application.Contacts.Commands;
using FluentValidation;

namespace Contax.Application.Contacts.Validators;

public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
{
    public CreateContactCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome é obrigatório.")
            .MinimumLength(3)
            .WithMessage("O nome deve ter pelo menos 3 caracteres.");
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("O email é obrigatório.")
            .EmailAddress()
            .WithMessage("O email deve ser válido.");
        RuleFor(x => x.Phone)
            .NotEmpty()
            .Matches(@"^\+?\d{10,15}$") // Adaptado para 10 a 15 dígitos (DDD + Número)
            .WithMessage(
                "O telefone deve conter apenas dígitos, opcionalmente com '+' no início, e ter entre 10 e 15 dígitos (Ex: +5541988887777)."
            );
    }
}
