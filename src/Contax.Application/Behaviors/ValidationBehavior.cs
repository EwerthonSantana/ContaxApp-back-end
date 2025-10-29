// Agenda.Application/Behaviors/ValidationBehavior.cs

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

// TRequest é o Command/Query (IRequest<TResponse>)
// TResponse é o tipo de retorno esperado
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    // O MediatR injeta todos os validadores que implementam IValidator<TRequest>
    // (aqueles que você registrou com AddValidatorsFromAssembly)
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        // Se não houver validadores para esta Request, avança
        if (!_validators.Any())
        {
            return await next();
        }

        // Validações em paralelo
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        // Agrupa todas as falhas de validação
        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
        {
            // Lança uma exceção para ser tratada pelo Controller
            // É comum criar uma exceção customizada, como DomainValidationException,
            // mas a ValidationException do FluentValidation é suficiente por agora.
            throw new ValidationException(failures);
        }

        // Se passar, segue para o Handler (o próximo passo no pipeline)
        return await next();
    }
}
