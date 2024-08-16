namespace Ticketing.Microservice.Tickets.DTOs;
public record TicketDto(Guid Id, string UserName, string Destination, decimal Price);