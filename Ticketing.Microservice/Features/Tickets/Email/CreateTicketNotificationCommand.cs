using MediatR;

namespace Ticketing.Microservice.Features.Tickets.Email
{
    public record CreateTicketNotificationCommand(string Email) : INotification;
}
