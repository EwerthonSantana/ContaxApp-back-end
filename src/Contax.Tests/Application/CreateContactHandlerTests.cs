using System.Threading;
using System.Threading.Tasks;
using Contax.Application.Contacts.Commands;
using Contax.Application.Contacts.Handlers;
using Contax.Domain.Entities;
using Contax.Domain.Interfaces;
using FluentValidation;
using Moq;
using Xunit;

public class CreateContactHandlerTests
{
    private readonly Mock<IContactRepository> _mockRepo;
    private readonly CreateContactCommand _validCommand;

    public CreateContactHandlerTests()
    {
        _mockRepo = new Mock<IContactRepository>();

        _validCommand = new CreateContactCommand("Teste Nome", "teste@email.com", "+5541999999999");
    }

    [Fact]
    public async Task Handle_ShouldCreateContact_WhenDataIsValid()
    {
        // ARRANGE: Configura o Repositório para retornar false (não existe)
        _mockRepo.Setup(r => r.ExistsByPhoneAsync(It.IsAny<string>(), null)).ReturnsAsync(false);

        var handler = new CreateContactHandler(_mockRepo.Object);

        // ACT
        var contactId = await handler.Handle(_validCommand, CancellationToken.None);

        // ASSERT
        // 1. Verifica se o método AddAsync foi chamado no Repositório (Write Model)
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Contact>()), Times.Once);

        // 3. Verifica se o ID retornado é válido
        Assert.NotEqual(Guid.Empty, contactId);
    }

    [Fact]
    public async Task Handle_ShouldTrowException_WhenPhoneAlreadyExists()
    {
        // ARRANGE: Configura o Repositório para retornar true (já existe)
        _mockRepo.Setup(r => r.ExistsByPhoneAsync(It.IsAny<string>(), null)).ReturnsAsync(true);

        var handler = new CreateContactHandler(_mockRepo.Object);

        // ACT & ASSERT
        // O handler deve lançar uma ValidationException (a nossa regra de negócio)
        await Assert.ThrowsAsync<ValidationException>(() =>
            handler.Handle(_validCommand, CancellationToken.None)
        );

        // Garante que o método AddAsync NUNCA foi chamado
        _mockRepo.Verify(r => r.AddAsync(It.IsAny<Contact>()), Times.Never);
    }
}
