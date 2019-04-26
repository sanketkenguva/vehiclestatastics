using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using VehicleStatastics.Models;

namespace VehicleStatastics
{
    public class VehicleHelper
    {
        List<VehicleReading> vehicleList = new List<VehicleReading>();
        List<Vehicle> vehicles = new List<Models.Vehicle>();

        public static VehicleHelper obj;
        public static VehicleHelper GetInstance()
        {
            if (obj == null)
            {
                obj = new VehicleHelper();
                obj.LoadJson();
                obj.FormatData();

            }
            return obj;

        }
        public void LoadJson()
        {
            using (StreamReader r = new StreamReader("C:\\Users\\sanket.kenguva\\Documents\\visual studio 2015\\Projects\\VehicleStatastics\\VehicleStatastics\\636917928435564285.json"))
            {
                string json = r.ReadToEnd();
                vehicleList = JsonConvert.DeserializeObject<List<VehicleReading>>(json);
            }
        }

        public void FormatData()
        {
            var query = vehicleList.GroupBy(x => x.VehicleId)
                   .Select(group =>
                         new {
                             id = group.Key,
                             Vehicles = group.OrderBy(x => x.TimeStamp)
                         });
            
            foreach (var group in query)
            {
                Vehicle vehicle = new Models.Vehicle();
                vehicle.VehicleId = group.id;
                vehicle.vehicleReadings = group.Vehicles.ToList();
                double totalspeed = 0;

                for (int i =1; i< vehicle.vehicleReadings.Count;i++)
                {
                    vehicle.TotalDistance += vehicle.vehicleReadings[i].OdoMeterReading - vehicle.vehicleReadings[i-1].OdoMeterReading;
                    vehicle.TotalFuelUsed += vehicle.vehicleReadings[i - 1].FuelLeft - vehicle.vehicleReadings[i].FuelLeft;
                    vehicle.vehicleReadings[i].DistanceTravelled = vehicle.vehicleReadings[i].OdoMeterReading - vehicle.vehicleReadings[i-1].OdoMeterReading;
                    vehicle.vehicleReadings[i].FuelUsed = vehicle.vehicleReadings[i-1].FuelLeft - vehicle.vehicleReadings[i].FuelLeft;
                    vehicle.vehicleReadings[i].Mileage = vehicle.vehicleReadings[i].DistanceTravelled / vehicle.vehicleReadings[i].FuelUsed;
                    vehicle.vehicleReadings[i].timeTaken = vehicle.vehicleReadings[i].TimeStamp - vehicle.vehicleReadings[i-1].TimeStamp;
                    vehicle.vehicleReadings[i].speed = vehicle.vehicleReadings[i].DistanceTravelled / vehicle.vehicleReadings[i].timeTaken.TotalHours;
                    totalspeed += vehicle.vehicleReadings[i].speed;
                }
                vehicle.AverageSpeed = totalspeed/ vehicle.vehicleReadings.Count;
                vehicles.Add(vehicle);
            }
        }

        public double GetAverageMileage(int vehicleid = 0)
        {
            Vehicle vehicle = vehicles.Where(x => x.VehicleId == vehicleid).FirstOrDefault();
             return vehicle.TotalDistance / vehicle.TotalFuelUsed;
        }

        public DateTime GetMileageHigh(int vehicleid=0)
        {            
            if (vehicleid == 0)
                return vehicleList.Where(x=>x.Mileage!=0).OrderBy(t => t.Mileage).FirstOrDefault().TimeStamp;
            else
                return vehicleList.Where(x => x.VehicleId == vehicleid && x.Mileage != 0).OrderBy(t => t.Mileage).FirstOrDefault().TimeStamp;

        }

        public DateTime GetMileageLow(int vehicleid = 0)
        {
            if (vehicleid == 0)
                return vehicleList.Where(x => x.Mileage != 0).OrderBy(t => t.Mileage).FirstOrDefault().TimeStamp ;
            else
                return vehicleList.Where(x => x.VehicleId == vehicleid && x.Mileage != 0).OrderByDescending(t => t.Mileage).FirstOrDefault().TimeStamp;

        }

        public double GetDistanceAverage(int vehicleid = 0)
        {
            Vehicle vehicle = vehicles.Where(x => x.VehicleId == vehicleid).FirstOrDefault();
            return vehicle.TotalDistance / vehicle.vehicleReadings.Count;
        }

        public long GetDistanceBest(int vehicleid = 0)
        {
                return vehicles.Where(x => x.VehicleId == vehicleid).Select(y => y.vehicleReadings).FirstOrDefault().OrderByDescending(z => z.DistanceTravelled).FirstOrDefault().DistanceTravelled;
        }

        public long GetDistanceWorst(int vehicleid = 0)
        {
            return vehicles.Where(x => x.VehicleId == vehicleid).Select(y => y.vehicleReadings).FirstOrDefault().Where(t=>t.DistanceTravelled!=0).OrderBy(z => z.DistanceTravelled).FirstOrDefault().DistanceTravelled;
        }

        public double GetTotalTimeAverageMaxDistance(int vehicleid = 0)
        {
            var vehicle = vehicles.Where(x => x.VehicleId == vehicleid).FirstOrDefault();
            double totalTime = 0;
            foreach (var reading in vehicle.vehicleReadings)
            {
                if (reading.DistanceTravelled == GetDistanceBest(vehicleid))
                {
                    totalTime += reading.timeTaken.TotalHours;
                }
            }
            return totalTime;
        }
        public double GetTotalTimeAverageMinDistance(int vehicleid = 0)
        {
            var vehicle = vehicles.Where(x => x.VehicleId == vehicleid).FirstOrDefault();
            double totalTime = 0;
            foreach (var reading in vehicle.vehicleReadings)
            {
                if (reading.DistanceTravelled == GetDistanceWorst(vehicleid))
                {
                    totalTime += reading.timeTaken.TotalHours;
                }
            }
            return totalTime;
        }
        public int GetBestVehicleByDistance()
        {
            return vehicles.OrderByDescending(x => x.TotalDistance).FirstOrDefault().VehicleId;
        }

        public int GetBestVehicleBySpeed()
        {
            return vehicles.OrderByDescending(x => x.AverageSpeed).FirstOrDefault().VehicleId;
        }

        public int GetBestVehicleByMileage()
        {
            return vehicles.OrderByDescending(x => x.AverageMileage).FirstOrDefault().VehicleId;
        }


    }
}