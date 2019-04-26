using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace VehicleStatastics.Models
{
    public class VehicleReading
    {
        public int VehicleId { get; set; }
        public DateTime TimeStamp { get; set; }
        public long OdoMeterReading { get; set; }

        public double FuelLeft { get; set; }

        public double Mileage { get; set; }

        public double FuelUsed { get; set; }

        public long DistanceTravelled { set; get; }

        public double speed { set; get; }
        public TimeSpan timeTaken { set; get; }
    }

    public class Vehicle
    {
        public List<VehicleReading> vehicleReadings = new List<VehicleReading>();

        public int VehicleId { get; set; }
        public long TotalDistance { get; set; }

        public double TotalFuelUsed { get; set; }

        public double AverageSpeed { get; set; }

        public double AverageMileage {
            get
            {
                return TotalDistance / TotalFuelUsed;
            }
        }

    }
}