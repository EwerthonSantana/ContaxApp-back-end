using System;
using MediatR;

public class UpdateContactCommand : IRequest<bool>
{
    public Guid Id { get; set; } // Identificador para quem atualizar
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
