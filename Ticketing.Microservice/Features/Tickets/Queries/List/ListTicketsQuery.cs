using Ticketing.Microservice.Tickets.DTOs;
using MediatR;

namespace Ticketing.Microservice.Features.Tickets.Queries.List;

public record ListTicketsQuery : IRequest<List<TicketDto>>;