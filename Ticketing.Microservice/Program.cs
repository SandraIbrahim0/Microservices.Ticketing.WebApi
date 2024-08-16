using MediatR;
using System.Reflection;
using Ticketing.Microservice.Core.Application.Contracts;
using Ticketing.Microservice.Features.Tickets.Commands.Create;
using Ticketing.Microservice.Features.Tickets.Email;
using Ticketing.Microservice.Features.Tickets.Queries.List;
using Ticketing.Microservice.Infrastructure.External;
using Ticketing.Microservice.Infrastructure.Persistence;
using Ticketing.Microservice.Tickets.Commands.Delete;
using Ticketing.Microservice.Tickets.Queries.Get;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddScoped<IMessageProducer, NotificationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/tickets/{id:guid}", async (Guid id, ISender mediatr) =>
{
    var ticket = await mediatr.Send(new GetTicketQuery(id));
    if (ticket == null) return Results.NotFound();
    return Results.Ok(ticket);
});

app.MapGet("/tickets", async (ISender mediatr) =>
{
    var tickets = await mediatr.Send(new ListTicketsQuery());
    return Results.Ok(tickets);
});

app.MapPost("/tickets", async (CreateTicketCommand command, IMediator mediatr) =>
{
    var ticketId = await mediatr.Send(command);
    if (Guid.Empty == ticketId) return Results.BadRequest();
      await mediatr.Publish(new CreateTicketNotificationCommand(command.Email));
    return Results.Created($"/tickets/{ticketId}", new { id = ticketId });
});

app.MapDelete("/tickets/{id:guid}", async (Guid id, ISender mediatr) =>
{
    await mediatr.Send(new DeleteTicketCommand(id));
    return Results.NoContent();
});

app.UseHttpsRedirection();

app.Run();
