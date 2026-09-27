using System.Reflection.Metadata;
using static OOP_Assignment_5.Program;

namespace OOP_Assignment_5
{
        #region Last Assignment code
        #region Interfaces
        public interface ITrackable
        {
            string GetTrackingStatus();
        }

        public interface IInsurable
        {
            decimal CalculateInsurance();
        }
        #endregion
        //================================================
        #region DeliveryAddress 
        public class DeliveryAddress
        {
            public string City;
            public string Street;
            public int BuildingNumber;

            public DeliveryAddress(string city, string street, int buildingNumber)
            {
                City = city;
                Street = street;
                BuildingNumber = buildingNumber;
            }
            public string GetFullAddress()
            {
                return $"{BuildingNumber} {Street}, {City}";
            }
        }
    #endregion
    //================================================
    //================================================
        #region Shipment class     
    public abstract class Shipment
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

        public Shipment(string trackingCode) : this(trackingCode, "Unknown", 1.0m, 50.0m, new DeliveryAddress("Default City", "Default St", 1))
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
    #endregion
    //================================================
    //================================================
        #region DeliveryCenter class (Edited)
    public class DeliveryCenter
        {
            private string centerName;
            private Shipment[] shipments;
            private int count;
            public DeliveryCenter(string centerName = "Main Center")
            {
                this.centerName = centerName;
                shipments = new Shipment[20];
                count = 0;
            }

            public Driver AssignedDriver { get; set; }
            public Shipment this[int index]
            {
                get
                {
                    if (shipments == null || index < 0 || index >= count)
                    {
                        return null;
                    }
                    return shipments[index];
                }
                set
                {
                    if (shipments != null && index >= 0 && index < count)
                    {
                        shipments[index] = value;
                    }
                }
            }


            public Shipment this[string trackingCode]
            {
                get
                {
                    if (shipments == null || string.IsNullOrWhiteSpace(trackingCode))
                    {
                        return null;
                    }

                    for (int i = 0; i < count; i++)
                    {
                        if (shipments[i].TrackingCode != null &&
                            shipments[i].TrackingCode.Equals(trackingCode, StringComparison.OrdinalIgnoreCase))
                        {
                            return shipments[i];
                        }
                    }

                    return null;
                }
            }

            public bool AddShipment(Shipment shipment)
            {
                if (shipments == null)
                {
                    shipments = new Shipment[20];
                }

                if (count >= 20 || shipment == null)
                {
                    return false;
                }

                shipments[count] = shipment;
                count++;
                return true;
            }
            public bool RemoveShipment(string trackingCode)
            {
                if (string.IsNullOrWhiteSpace(trackingCode) || count == 0)
                {
                    return false;
                }

                int indexToRemove = -1;
                for (int i = 0; i < count; i++)
                {
                    if (shipments[i]?.TrackingCode != null &&
                        shipments[i].TrackingCode.Equals(trackingCode, StringComparison.OrdinalIgnoreCase))
                    {
                        indexToRemove = i;
                        break;
                    }
                }
                if (indexToRemove == -1)
                {
                    return false;
                }
                for (int i = indexToRemove; i < count - 1; i++)
                {
                    shipments[i] = shipments[i + 1];
                }
                shipments[count - 1] = null;
                count--;

                return true;
            }

            public void PrintAllShipments()
            {
                Console.WriteLine($"--- Delivery Center: {centerName} ---");
                Console.WriteLine($"Total Shipments: {count} / 20");
                Console.WriteLine(new string('=', 30));

                if (count == 0)
                {
                    Console.WriteLine("No shipments available in this center.");
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (shipments[i] != null)
                        {
                            shipments[i].PrintShipment();
                        }
                    }
                }
            }

            public void PrintTrackingStatuses()
            {
                for (int i = 0; i < count; i++)
                {
                    if (shipments[i] is ITrackable trackable)
                    {
                        Console.WriteLine(trackable.GetTrackingStatus());
                    }
                }
            }
        }
        #endregion
        //================================================
        #region StandardShipment Class
        public class StandardShipment : Shipment, ITrackable, IInsurable
        {
            public StandardShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination)
                : base(trackingCode, description, weight, deliveryFee, destination)
            {
            }

            public StandardShipment(string trackingCode)
                : base(trackingCode)
            {
            }


            public override decimal EstimatedCost
            {
                get
                {
                    return DeliveryFee + (Weight * 5m);
                }
            }
            public override Shipment CopyShipment()
            {
                return new StandardShipment(TrackingCode, Description, Weight, DeliveryFee, Destination);
            }

            public override Shipment DeepCopy()
            {
                DeliveryAddress clonedAddress = new DeliveryAddress(Destination.City, Destination.Street, Destination.BuildingNumber);
                return new StandardShipment(TrackingCode, Description, Weight, DeliveryFee, clonedAddress);
            }
            public override void PrintShipment()
            {
                Console.WriteLine($"Tracking Code: {TrackingCode}");
                Console.WriteLine($"Description: {Description}");
                Console.WriteLine($"Weight: {Weight}");
                Console.WriteLine($"Delivery Fee: {DeliveryFee}");
                Console.WriteLine($"Destination: {Destination.GetFullAddress()}");
                Console.WriteLine($"Estimated Cost: {EstimatedCost}");
                Console.WriteLine(new string('-', 30));
            }
            public string GetTrackingStatus()
            {
                return $"Shipment {TrackingCode} is Ready.";
            }

            public decimal CalculateInsurance()
            {
                return EstimatedCost * 0.05m;
            }
        }
        #endregion
        //================================================
        #region ExpressShipment Class
        public class ExpressShipment : Shipment, ITrackable, IInsurable
        {
            private decimal extraFee;

            public decimal ExtraFee
            {
                get { return extraFee; }
                set
                {
                    if (value >= 0)
                    {
                        extraFee = value;
                    }
                }
            }

            public ExpressShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination, decimal extraFee)
                : base(trackingCode, description, weight, deliveryFee, destination)
            {
                ExtraFee = extraFee >= 0 ? extraFee : 0m;
            }
            public override decimal EstimatedCost
            {
                get
                {
                    return DeliveryFee + (Weight * 5m) + ExtraFee;
                }
            }
            public override Shipment CopyShipment()
            {
                return new ExpressShipment(TrackingCode, Description, Weight, DeliveryFee, Destination, ExtraFee);
            }

            public override Shipment DeepCopy()
            {
                DeliveryAddress clonedAddress = new DeliveryAddress(Destination.City, Destination.Street, Destination.BuildingNumber);
                return new ExpressShipment(TrackingCode, Description, Weight, DeliveryFee, clonedAddress, ExtraFee);
            }
            public override void PrintShipment()
            {
                Console.WriteLine($"Tracking Code: {TrackingCode}");
                Console.WriteLine($"Description: {Description}");
                Console.WriteLine($"Weight: {Weight}");
                Console.WriteLine($"Delivery Fee: {DeliveryFee}");
                Console.WriteLine($"Extra Fee: {ExtraFee}");
                Console.WriteLine($"Destination: {Destination.GetFullAddress()}");
                Console.WriteLine($"Estimated Cost: {EstimatedCost}");
                Console.WriteLine(new string('-', 30));
            }
            public string GetTrackingStatus()
            {
                return $"Shipment {TrackingCode} is Out for Delivery.";
            }

            public decimal CalculateInsurance()
            {
                return EstimatedCost * 0.08m;
            }
        }
        #endregion
        //================================================
        #region InternationalShipment Class
        public class InternationalShipment : Shipment, ITrackable, IInsurable
        {
            private string destinationCountry;
            private decimal customsFee;

            public string DestinationCountry
            {
                get { return destinationCountry; }
                set
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        destinationCountry = value;
                    }
                }
            }

            public decimal CustomsFee
            {
                get { return customsFee; }
                set
                {
                    if (value >= 0)
                    {
                        customsFee = value;
                    }
                }
            }

            public InternationalShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination, string destinationCountry, decimal customsFee)
                : base(trackingCode, description, weight, deliveryFee, destination)
            {
                DestinationCountry = string.IsNullOrWhiteSpace(destinationCountry) ? "Unknown" : destinationCountry;
                CustomsFee = customsFee >= 0 ? customsFee : 0m;
            }

            public override decimal EstimatedCost
            {
                get
                {
                    return DeliveryFee + (Weight * 5m) + CustomsFee;
                }
            }

            public virtual void GenerateCustomsReport()
            {
                Console.WriteLine($"Generating standard customs report : {DestinationCountry}");
            }

            public override Shipment CopyShipment()
            {
                return new InternationalShipment(TrackingCode, Description, Weight, DeliveryFee, Destination, DestinationCountry, CustomsFee);
            }
            public override Shipment DeepCopy()
            {
                DeliveryAddress clonedAddress = new DeliveryAddress(Destination.City, Destination.Street, Destination.BuildingNumber);
                return new InternationalShipment(TrackingCode, Description, Weight, DeliveryFee, clonedAddress, DestinationCountry, CustomsFee);
            }
            public override void PrintShipment()
            {
                Console.WriteLine($"Tracking Code: {TrackingCode}");
                Console.WriteLine($"Description: {Description}");
                Console.WriteLine($"Weight: {Weight}");
                Console.WriteLine($"Delivery Fee: {DeliveryFee}");
                Console.WriteLine($"Destination Country: {DestinationCountry}");
                Console.WriteLine($"Customs Fee: {CustomsFee}");
                Console.WriteLine($"Destination: {Destination.GetFullAddress()}");
                Console.WriteLine($"Estimated Cost: {EstimatedCost}");
                Console.WriteLine(new string('-', 30));
            }
            public string GetTrackingStatus()
            {
                return $"Shipment {TrackingCode} has been Delivered.";
            }

            public decimal CalculateInsurance()
            {
                return EstimatedCost * 0.12m;
            }
        }
        #endregion
        //================================================
        #region CompletedShipment
        public sealed class CompletedShipment : Shipment
        {
            public CompletedShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination)
                : base(trackingCode, description, weight, deliveryFee, destination)
            {
            }

            public CompletedShipment(string trackingCode)
                : base(trackingCode)
            {
            }
            public override decimal EstimatedCost
            {
                get
                {
                    return DeliveryFee + (Weight * 5m);
                }
            }

            public override Shipment CopyShipment()
            {
                return new CompletedShipment(TrackingCode, Description, Weight, DeliveryFee, Destination);
            }
            public override Shipment DeepCopy()
            {
                DeliveryAddress clonedAddress = new DeliveryAddress(Destination.City, Destination.Street, Destination.BuildingNumber);
                return new CompletedShipment(TrackingCode, Description, Weight, DeliveryFee, clonedAddress);
            }
            public override void PrintShipment()
            {
                Console.WriteLine("[Status: Completed Shipment]");
                Console.WriteLine($"Tracking Code: {TrackingCode}");
                Console.WriteLine($"Description: {Description}");
                Console.WriteLine($"Weight: {Weight}");
                Console.WriteLine($"Delivery Fee: {DeliveryFee}");
                Console.WriteLine($"Destination: {Destination.GetFullAddress()}");
                Console.WriteLine($"Estimated Cost: {EstimatedCost}");
                Console.WriteLine(new string('-', 30));
            }
        }
        #endregion
        //================================================
        #region PriorityInternationalShipment
        public class PriorityInternationalShipment : InternationalShipment
        {
            public PriorityInternationalShipment(string trackingCode, string description, decimal weight, decimal deliveryFee, DeliveryAddress destination, string destinationCountry, decimal customsFee)
                : base(trackingCode, description, weight, deliveryFee, destination, destinationCountry, customsFee)
            {
            }
            public sealed override void GenerateCustomsReport()
            {
                Console.WriteLine($"[PRIORITY EXPEDITED] Generating express customs report for {DestinationCountry}");
            }
            public override Shipment CopyShipment()
            {
                return new PriorityInternationalShipment(TrackingCode, Description, Weight, DeliveryFee, Destination, DestinationCountry, CustomsFee);
            }
            public override Shipment DeepCopy()
            {
                DeliveryAddress clonedAddress = new DeliveryAddress(Destination.City, Destination.Street, Destination.BuildingNumber);
                return new PriorityInternationalShipment(TrackingCode, Description, Weight, DeliveryFee, clonedAddress, DestinationCountry, CustomsFee);
            }
            public override void PrintShipment()
            {
                Console.WriteLine("[Priority International Shipment]");
                base.PrintShipment();
            }
        }
        #endregion
        //================================================
        #region DeliveryReport Class
        public static class DeliveryReport
        {
            public static void PrintShipment(ITrackable shipment)
            {
                if (shipment != null)
                {
                    Console.WriteLine(shipment.GetTrackingStatus());
                }
            }

            public static void PrintInsurance(IInsurable shipment)
            {
                if (shipment != null)
                {
                    Console.WriteLine($"Insurance Cost: {shipment.CalculateInsurance()}");
                }
            }
        }
        #endregion
        //================================================
        #region Create DeliveryHelper
        public static class DeliveryHelper
        {
            public static void PrintShipmentDetails(Shipment shipment)
            {
                if (shipment != null)
                {
                    shipment.PrintShipment();
                }
                else
                {
                    Console.WriteLine("The shipment details cannot be displayed because the shipment is null.");
                }
            }
        }
        #endregion
        //================================================
        #region Driver
        public class Driver
        {
            public string Name { get; set; }

            public Driver(string name)
            {
                Name = name;
            }
        }
        #endregion
        //================================================
        #region DeliveryUtilityies Class
        public static class DeliveryUtilities
        {
            public static void PrintSeparator()
            {
                Console.WriteLine("=============================================");
            }
            public static void PrintSystemTitle()
            {
                PrintSeparator();
                Console.WriteLine("Delivery Center");
                PrintSeparator();
            }
        }
        #endregion
        //================================================
        #region ShipmentExtensions 
        public static class ShipmentExtensions
        {
            public static string GetSummary(this Shipment shipment)
            {
                if (shipment == null)
                {
                    return "Null Shipment";
                }
                string trackingCode = shipment.TrackingCode;
                string shipmentType = shipment.GetType().Name.Replace("Shipment", "");
                string weightStr = $"{shipment.Weight} KG";

                string status = "Unknown";
                if (shipment is ITrackable trackable)
                {
                    string fullStatus = trackable.GetTrackingStatus();
                    if (fullStatus.Contains("Delivered", StringComparison.OrdinalIgnoreCase))
                    {
                        status = "Delivered";
                    }
                    else if (fullStatus.Contains("Out for Delivery", StringComparison.OrdinalIgnoreCase))
                    {
                        status = "Out for Delivery";
                    }
                    else if (fullStatus.Contains("Ready", StringComparison.OrdinalIgnoreCase))
                    {
                        status = "Ready";
                    }
                    else
                    {
                        status = fullStatus;
                    }
                }

                return $"{trackingCode} | {shipmentType} | {weightStr} | {status}";
            }

            public static bool IsDelivered(this Shipment shipment)
            {
                if (shipment is ITrackable trackable)
                {
                    string status = trackable.GetTrackingStatus();
                    return status.Contains("Delivered", StringComparison.OrdinalIgnoreCase);
                }

                return false;
            }
        }
        #endregion
        //================================================
        #endregion
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Q1
            // Q1 Object Copying
            // a) What happens when you assign one object variable to another object variable?
            // both variables refer to the same object in memory.

            // b) Does assigning one object to another create a new object? Explain.
            // No,It only copies Make botth of them refer to the same objet.

            // c) What is the difference between copying an object and copying its reference?
            // Copying an object creates a new object with the same values, while copying a reference only creates another reference to the same object.
            #endregion

            #region Q2
            // Shallow Copy vs Deep Copy
            //a) What is a Shallow Copy?
            // it only copies the values of the original object. If the original object has reference-type fields, the shallow copy will still refer to the same objects in memory as the original object.

            //b) What is a Deep Copy?
            // creatiing a new object that is a copy of the original object.

            //c) What happens to reference-type members when a Shallow Copy is created?
            // They are not copied and will refer to the same objects in memory as the original object.

            //d) What happens to reference-type members when a Deep Copy is created?
            // Reference-type members are copied too, creating new instances of the referenced objects.

            //e) Give one situation where Deep Copy would be safer than Shallow Copy.
            // when you want to ensure that modifications to the copied object do not affect the original object or any other objects that reference it.
            #endregion

            #region Q3
            // Static Members
            //a) What is a static field, and how is it different from an instance field ?
            // A static is a variable that is shared among the class, instance field is unique.
            // Static fields are associated with the class itself.

            //b) What is a static method? Can a static method directly access instance members?
            // A static method belongs to the class not object in the class.
            // It can be called without creating an object of the class.
            // cannot directly access instance members because it does not have a reference to a specific instance of the class.

            //c) What is a static constructor, and when is it executed ?
            // A static constructor is used to initialize static members of a class.
            // It is executed only once, when the class is first accessed

            //d) What is a static class? Can you create an object from a static class?
            // A static class is a class that cannot be instantiated.
            // You cannot create an object from a static class.
            #endregion

            #region Q4
            //Extension Methods
            // a) What is an Extension Method?
            // A static method that can be called as if it were an instance method of the extended class.

            //b) What keyword must be used in the first parameter of an extension method?
            // this 

            //c) Where must an extension method be declared?
            // in a static class.

            //d) Can an extension method access private members of the class it extends?
            // Yes.
            #endregion

            #region Q5
            //a) What is a Partial Class?
            // class that can be split into multiple files, allowing for better organization.

            //b) Why would a developer split one class into multiple files?
            // To improve code organization, maintainability, and collaboration.

            //c) What is a Partial Method?
            // method that can be declared in one part of a partial class and implemented in another part of the same class.

            //d) What happens if a declared partial method has no implementation?
            // it is removed by the compiler.
            #endregion

        
        }
    }
}
