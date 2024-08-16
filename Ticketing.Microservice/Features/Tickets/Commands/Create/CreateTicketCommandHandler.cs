using Ticketing.Microservice.Domain;
using MediatR;
using Ticketing.Microservice.Infrastructure.Persistence;

namespace Ticketing.Microservice.Features.Tickets.Commands.Create;

public class CreateTicketCommandHandler(AppDbContext context) : IRequestHandler<CreateTicketCommand, Guid>
{
    public async Task<Guid> Handle(CreateTicketCommand command, CancellationToken cancellationToken)
    {
        var ticket = new Ticket(command.UserName, command.Destination, command.Email, command.Price);
        await context.Tickets.AddAsync(ticket);
        await context.SaveChangesAsync();
        return ticket.Id;
    }
}
