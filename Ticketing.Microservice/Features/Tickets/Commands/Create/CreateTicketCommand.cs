using MediatR;

namespace Ticketing.Microservice.Features.Tickets.Commands.Create;

public record CreateTicketCommand(string UserName, string Destination, string Email, decimal Price) : IRequest<Guid>;