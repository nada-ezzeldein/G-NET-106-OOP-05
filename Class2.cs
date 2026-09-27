using System;
using System.Reflection.Metadata;
using static OOP_Assignment_5.Program;
namespace OOP_Assignment_5
public abstract partial class Shipment
{
    private string trackingStatus = "Ready";
    partial void OnTrackingStatusChanged(string newStatus);
    public string TrackingStatusProperty
    {
        get { return trackingStatus; }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                trackingStatus = value;
            }
        }
    }

    public virtual string GetTrackingStatus()
    {
        return $"Shipment {TrackingCode} status: {trackingStatus}";
    }

    public void UpdateTrackingStatus(string newStatus)
    {
        if (!string.IsNullOrWhiteSpace(newStatus))
        {
            trackingStatus = newStatus;
            OnTrackingStatusChanged(newStatus);
        }
    }
    partial void OnTrackingStatusChanged(string newStatus)
    {
        Console.WriteLine($"Tracking status changed to: {newStatus}");
    }
}