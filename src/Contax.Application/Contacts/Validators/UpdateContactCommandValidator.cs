// Contax.Application/Validators/UpdateContactCommandValidator.cs
using FluentValidation;

public class UpdateContactCommandValidator : AbstractValidator<UpdateContactCommand>
{
    private const string StrictEmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$";

    public UpdateContactCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("O ID do contato é obrigatório para atualização.")
            .Must(id => id != Guid.Empty)
            .WithMessage("O ID do contato não pode ser vazio.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome é obrigatório.")
            .MinimumLength(3)
            .WithMessage("O nome deve ter pelo menos 3 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("O email é obrigatório.")
             .Matches(StrictEmailRegex)
            .WithMessage("O email deve ser um endereço válido (ex: usuario@dominio.com)."); RuleFor(x => x.Phone);

        RuleFor(x => x.Phone)
            .NotEmpty()
            .Matches(@"^\+?\d{10,15}$") // Adaptado para 10 a 15 dígitos (DDD + Número)
            .WithMessage(
                "O telefone deve conter apenas dígitos, opcionalmente com '+' no início, e ter entre 10 e 15 dígitos (Ex: +5541988887777)."
            );
    }
}
