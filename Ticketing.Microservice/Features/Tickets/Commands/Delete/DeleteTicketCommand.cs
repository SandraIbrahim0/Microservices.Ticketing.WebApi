using MediatR;

namespace Ticketing.Microservice.Tickets.Commands.Delete;

public record DeleteTicketCommand(Guid Id) : IRequest;
