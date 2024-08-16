using MediatR;

namespace Ticketing.Microservice.Features.Tickets.Commands.Update
{
    public record UpdateTicketCommand(string UserName, string Destination, string Email, decimal Price) : IRequest;
}
