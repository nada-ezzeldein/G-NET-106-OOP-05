using System;
using System.Reflection.Metadata;
using static OOP_Assignment_5.Program;
namespace OOP_Assignment_5
public abstract partial class Shipment
{
    public static int TotalShipmentsCreated { get; private set; }
    static Shipment()
    {
        TotalShipmentsCreated = 0;
        Console.WriteLine("Shipment System Initialized");
    }
    public static int GetTotalShipmentsCreated()
    {
        return TotalShipmentsCreated;
    }
    private string trackingCode;
    private string description;
    private decimal weight;
    private decimal deliveryFee;

    public Shipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination)
    {
        this.trackingCode = string.IsNullOrWhiteSpace(trackingCode) ? "UNVALID" : trackingCode;
        this.description = string.IsNullOrWhiteSpace(description) ? "UNVALID" : description;
        this.weight = weight > 0 ? weight : 1.0m;
        this.deliveryFee = deliveryFee > 0 ? deliveryFee : 10.0m;
        Destination = destination;
        TotalShipmentsCreated++;
    }

    public Shipment(string trackingCode)
        : this(trackingCode, "Unknown", 1.0m, 50.0m, new DeliveryAddress("Default City", "Default St", 1))
    {
    }

    public abstract Shipment CopyShipment();

    public Shipment ShallowCopy()
    {
        return (Shipment)this.MemberwiseClone();
    }

    public abstract Shipment DeepCopy();

    public string TrackingCode
    {
        get { return trackingCode; }
        private set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                trackingCode = value;
            }
        }
    }

    public string Description
    {
        get { return description; }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                description = value;
            }
        }
    }

    public decimal Weight
    {
        get { return weight; }
        set
        {
            if (value > 0)
            {
                weight = value;
            }
        }
    }

    public decimal DeliveryFee
    {
        get { return deliveryFee; }
        private set
        {
            if (value > 0)
            {
                deliveryFee = value;
            }
        }
    }

    public DeliveryAddress Destination { get; set; }
    public abstract decimal EstimatedCost { get; }
    public abstract void PrintShipment();
    public void UpdateDeliveryFee(decimal newFee)
    {
        if (newFee > 0)
        {
            deliveryFee = newFee;
        }
    }
    public void UpdateWeight(decimal newWeight)
    {
        if (newWeight > 0)
        {
            weight = newWeight;
        }
    }
    public void UpdateWeight(decimal baseWeight, decimal extraPackingWeight)
    {
        decimal totalWeight = baseWeight + extraPackingWeight;
        if (totalWeight > 0)
        {
            weight = totalWeight;
        }
    }
}