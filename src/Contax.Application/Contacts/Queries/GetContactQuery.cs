using MediatR;

public class GetContactByIdQuery : IRequest<ContactDTO>
{
    public Guid Id { get; set; }
}
