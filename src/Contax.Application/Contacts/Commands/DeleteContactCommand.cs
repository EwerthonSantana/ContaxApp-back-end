using MediatR;

public class DeleteContactCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
