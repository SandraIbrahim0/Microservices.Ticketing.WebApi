using MediatR;
using Ticketing.Microservice.Tickets.Commands.Delete;
using Ticketing.Microservice.Infrastructure.Persistence;

namespace CQRSMediatR.Features.Tickets.Commands.Delete;

public class DeleteTicketCommandHandler(AppDbContext context) : IRequestHandler<DeleteTicketCommand>
{
    public async Task Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Tickets.FindAsync(request.Id);
        if (product is null) return;
        context.Tickets.Remove(product);
        await context.SaveChangesAsync();
    }
}
