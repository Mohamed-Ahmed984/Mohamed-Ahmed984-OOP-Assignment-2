namespace SrpLab;

public class TicketSla
{
    private SupportTicket _ticket;

    public TicketSla(SupportTicket ticket)
    {
        _ticket = ticket;
    }

    public DateTimeOffset SlaDeadline()
    {
        int hours = 72;
        if (_ticket.Priority == "P1")
            hours = 4;
        else if (_ticket.Priority == "P2")
            hours = 24;
        return _ticket.OpenedAt.AddHours(hours);
    }

    public bool IsBreached(DateTimeOffset now)
    {
        return now > SlaDeadline();
    }
}
