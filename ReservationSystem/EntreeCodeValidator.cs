using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

public class EntreeCodeValidator
{
    private List<Ticket> tickets;

    public EntreeCodeValidator()
    {
        // Load the tickets from the JSON file
        tickets = Ticket.LoadTicketsFromFile("./JSON-Files/OnlineTickets.json");
    }

    public bool IsCodeValid(string code)
    {
        // Check if any ticket has the given code
        return tickets.Any(t => t.TicketCode == code);
    }

    // Additional method to get ticket by code if needed
    public Ticket GetTicketByCode(string code)
    {
        return tickets.FirstOrDefault(t => t.TicketCode == code);
    }

}
