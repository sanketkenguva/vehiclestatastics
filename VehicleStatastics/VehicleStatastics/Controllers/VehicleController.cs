using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VehicleStatastics.Controllers
{
    public class VehicleController : Controller
    {
        VehicleHelper helper = VehicleHelper.GetInstance();
        // GET: Vehicle
        public ActionResult Index()
        {
            return View();
        }

        public double GetAverageMileage(int vehicleid = 0)
        {
            return helper.GetAverageMileage(vehicleid);
        }

        public DateTime GetMileageHigh(int vehicleid = 0)
        {
            return helper.GetMileageHigh(vehicleid);
        }

        public DateTime GetMileageLow(int vehicleid = 0)
        {
            return helper.GetMileageLow(vehicleid);
        }

        public double GetDistanceAverage(int vehicleid = 0)
        {
            return helper.GetDistanceAverage(vehicleid);
        }

        public long GetDistanceBest(int vehicleid = 0)
        {
            return helper.GetDistanceBest(vehicleid);
        }

        public long GetDistanceWorst(int vehicleid = 0)
        {
            return helper.GetDistanceWorst(vehicleid);
        }

        public double GetAverageMaxDistance(int vehicleid = 0)
        {
            return helper.GetTotalTimeAverageMaxDistance(vehicleid);
        }

        public double GetAverageMinDistance(int vehicleid = 0)
        {
            return helper.GetTotalTimeAverageMinDistance(vehicleid);
        }

        public int GetBestVehicleByDistance()
        {
            return helper.GetBestVehicleByDistance();
        }

        public int GetBestVehicleBySpeed()
        {
            return helper.GetBestVehicleBySpeed();
        }

        public int GetBestVehicleByMileage()
        {
            return helper.GetBestVehicleByMileage();
        }
    }
}