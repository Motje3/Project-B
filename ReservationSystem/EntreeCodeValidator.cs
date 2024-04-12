using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

public static class EntreeCodeValidator
{
    public static bool IsCodeValid(string code)
    {
        // Check if any ticket has the given code
        return Ticket.Tickets.Any(t => t.TicketCode == code);
    }

    // Additional method to get ticket by code if needed
    public static Ticket GetTicketByCode(string code)
    {
        return Ticket.Tickets.FirstOrDefault(t => t.TicketCode == code);
    }

}
