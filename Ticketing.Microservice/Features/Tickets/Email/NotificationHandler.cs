using MediatR;
using Shared;
using Ticketing.Microservice.Core.Application.Contracts;
using Ticketing.Microservice.Features.Tickets.Email;

namespace Ticketing.Microservice.Features.Tickets.Notifications;

public class NotficationHandler: INotificationHandler<CreateTicketNotificationCommand>
{
    private readonly IMessageProducer messageProducer;

    public NotficationHandler(IMessageProducer messageProducer)
    {
        this.messageProducer = messageProducer; 
    }

    public async Task Handle(CreateTicketNotificationCommand notification, CancellationToken cancellationToken)
    {
        string message = "Ticket has been booked Please wait the confiramtion e-mail";
        var emailModelToParse = new EmailModelToParse { Content = message ,Sender ="sandra.ibrahim00@gmail.com" , Recipient = notification.Email, Subject="Rabbit MQ test"};
        messageProducer.SendMessage(emailModelToParse);
    }
}
