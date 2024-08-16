namespace Ticketing.Microservice.Domain;

public class Ticket
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public DateTime BookedOn { get; set; }
    public string Destination { get; set; }
    public string Email { get; set; }
    public decimal Price { get; set; }


    public Ticket(string userName, string destination , string email , decimal price)
    {
        Id = Guid.NewGuid();
        UserName = userName;
        BookedOn = DateTime.Now;
        Email = email;
        Destination = destination;
        Price = price;
    }
}