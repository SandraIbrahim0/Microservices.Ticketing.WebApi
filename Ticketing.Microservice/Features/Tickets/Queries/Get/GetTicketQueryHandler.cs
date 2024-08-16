using MediatR;
using Ticketing.Microservice.Tickets.DTOs;
using Ticketing.Microservice.Tickets.Queries.Get;
using Ticketing.Microservice.Infrastructure.Persistence;

namespace Ticketing.Microservice.Features.Tickets.Queries.Get;

public class GetTicketQueryHandler(AppDbContext context) : IRequestHandler<GetTicketQuery, TicketDto?>
{
    public async Task<TicketDto?> Handle(GetTicketQuery request, CancellationToken cancellationToken)
    {
        var ticket = await context.Tickets.FindAsync(request.Id);
        if (ticket is null) return null;
        return new TicketDto(ticket.Id, ticket.UserName, ticket.Destination, ticket.Price);
    }
}
