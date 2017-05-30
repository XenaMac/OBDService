using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeService
{
    public static class globalData
    {
        public static List<canCodeBase> canCodeBases = new List<canCodeBase>();
        public static List<pidRequest> pidRequests = new List<pidRequest>();
        public static List<pidResponse> pidResponses = new List<pidResponse>();
        public static List<vehicleService> vehicleServices = new List<vehicleService>();
        public static List<vehicleClassCAN> vehicleClassCANs = new List<vehicleClassCAN>();
        public static List<serviceLookup> serviceLookups = new List<serviceLookup>();
        public static List<vehicleVehicleClass> vehicleVehicleClasses = new List<vehicleVehicleClass>();
        public static List<vehicleClass> vehicleClasses = new List<vehicleClass>();
        public static List<accelVehicleClass> accelVehicleClasses = new List<accelVehicleClass>();
        public static List<accelVals> accelValues = new List<accelVals>();
        public static List<vehicle> vehicles = new List<vehicle>();

        public static void updateIntelliStuffBehavior(string MACAddress, string behavior) {
            try {
                var lSrv = from v in globalData.vehicles
                           join vc in globalData.vehicleServices on v.vehicleID equals vc.vehicleID
                           join sl in globalData.serviceLookups on vc.serviceLookupID equals sl.serviceLookupID
                           where v.MACAddress == MACAddress
                           select sl;
                string connStr = string.Empty;
                string srvURL = string.Empty;
                foreach (serviceLookup sl in lSrv)
                {
                    connStr = sl.connString;
                    srvURL = sl.serviceURL;
                }
                if (!string.IsNullOrEmpty(srvURL))
                {
                    srTruckService.CANCodeInterfaceClient srv = new srTruckService.CANCodeInterfaceClient();
                    srv.Endpoint.Address = new System.ServiceModel.EndpointAddress(srvURL);
                    srv.addDriverBehavior(MACAddress, behavior);
                }
            }
            catch (Exception ex) {
                string err = ex.ToString();
                SQLCode sql = new SQLCode();
                sql.logError(ex.ToString(), "updateIntelliStuffBehavior");
                sql = null;
            }
        }
    }
}