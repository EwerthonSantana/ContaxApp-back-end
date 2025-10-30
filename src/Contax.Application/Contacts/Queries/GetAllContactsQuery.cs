using MediatR;

public class GetAllContactsQuery : IRequest<IEnumerable<ContactDTO>> { }
