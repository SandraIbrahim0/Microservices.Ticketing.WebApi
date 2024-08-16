using MediatR;
using Ticketing.Microservice.Infrastructure.Persistence;

namespace Ticketing.Microservice.Features.Tickets.Commands.Update
{
    public class UpdateTicketCommandHandler(AppDbContext context) : IRequestHandler<UpdateTicketCommand>
    {
        public async Task Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = await context.Tickets.FindAsync(request.Email);
            if (ticket is null) return;
            context.Tickets.Update(ticket);
            await context.SaveChangesAsync();
        }
    }
}
