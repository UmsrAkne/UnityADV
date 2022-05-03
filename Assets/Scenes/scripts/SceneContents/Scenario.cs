using System.Collections.Generic;

public class Scenario
{
    public string Text { get; set; }

    public List<ImageOrder> ImageOrders { get; set; } = new List<ImageOrder>();

    public int VoiceIndex { get; set; }
}
