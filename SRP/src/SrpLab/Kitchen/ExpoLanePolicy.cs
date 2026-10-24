namespace SrpLab;

public class ExpoLanePolicy
{
    private KitchenTicket _ticket;

    public ExpoLanePolicy(KitchenTicket ticket)
    {
        _ticket = ticket;
    }

    public string ExpoLaneHint()
    {
        return new AllergenDetector(_ticket).DetectAllergens().Count > 0 ? "LANE-ALLERGY" : new KitchenReadyEstimate(_ticket).EstimatedReadyMinutes(2) > 20 ? "LANE-SLOW" : "LANE-FAST";
    }
}
