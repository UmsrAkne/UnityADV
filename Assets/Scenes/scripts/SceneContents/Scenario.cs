using System.Collections.Generic;

public class Scenario
{
    public string Text { get; set; }

    public List<ImageOrder> ImageOrders { get; set; } = new List<ImageOrder>();

    public List<ImageOrder> DrawOrders { get; set; } = new List<ImageOrder>();

    public List<VoiceOrder> VoiceOrders { get; set; } = new List<VoiceOrder>();

    public int VoiceIndex { get; set; }
}
