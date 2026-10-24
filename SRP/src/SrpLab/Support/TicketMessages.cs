namespace SrpLab;

public class TicketMessages
{
    private SupportTicket _ticket;

    public TicketMessages(SupportTicket ticket)
    {
        _ticket = ticket;
    }

    public string DraftPublicReply(string agentName)
    {
        var apology = _ticket.Priority == "P1" ? "We are treating this as a critical incident." : "Thanks for reaching out.";
        return $"Hi,\n{apology}\nTicket {_ticket.Id} is with {agentName}. Next update before {new TicketSla(_ticket).SlaDeadline():u}.\n";
    }

    public string InternalEscalationBlurb()
    {
        return $"ESCALATE {_ticket.Id} priority={_ticket.Priority} breachAt={new TicketSla(_ticket).SlaDeadline():u} keywords-scanned=yes";
    }
}
