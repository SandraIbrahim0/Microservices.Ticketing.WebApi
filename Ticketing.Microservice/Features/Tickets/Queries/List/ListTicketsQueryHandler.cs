using MediatR;
using Microsoft.EntityFrameworkCore;
using Ticketing.Microservice.Features.Tickets.Queries.List;
using Ticketing.Microservice.Infrastructure.Persistence;
using Ticketing.Microservice.Tickets.DTOs;

namespace CQRSMediatR.Features.Products.Queries.List;

public class ListTicketsQueryHandler(AppDbContext context) : IRequestHandler<ListTicketsQuery, List<TicketDto>>
{
    public async Task<List<TicketDto>> Handle(ListTicketsQuery request, CancellationToken cancellationToken)
    {
        return await context.Tickets
            .Select(p => new TicketDto(p.Id, p.UserName, p.Destination, p.Price))
            .ToListAsync();
    }
}
