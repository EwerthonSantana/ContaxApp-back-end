using MediatR;

// Retorna uma coleção de ContatoDTO
public class GetAllContactsQuery : IRequest<IEnumerable<ContactDTO>>
{
    // Não precisa de propriedades
}
