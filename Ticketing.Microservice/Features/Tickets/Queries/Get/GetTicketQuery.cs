using MediatR;
using Ticketing.Microservice.Tickets.DTOs;

namespace Ticketing.Microservice.Tickets.Queries.Get;

public record GetTicketQuery(Guid Id) : IRequest<TicketDto>;