using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeService
{
    public class canObject
    {
        public List<reqRes> can { get; set; }
        public string c { get; set; }
    }

    public class reqRes {
        public string rr { get; set; }
    }

    public class settingObject {
        public int getCodeRateSecs { get; set; }
        public int udpTimeoutSecs { get; set; }
        public int wifiApnTimeoutSecs { get; set; }
        public int wifiClientTimeoutSecs { get; set; }
        public string sftpIPAddress { get; set; }
    }

    public class pidRequest {
        public Guid requestID { get; set; }
        public Guid canCodeBaseID { get; set; }
        public string requestCode { get; set; }
        public string requestName { get; set; }
    }

    public class pidResponse {
        public Guid responseID { get; set; }
        public Guid requestID { get; set; }
        public string responseCode { get; set; }
        public int responseType { get; set; }
    }

    public class canCodeBase {
        public Guid canCodeBaseID { get; set; }
        public string canCodeBaseName { get; set; }
        public string make { get; set;}
        public string model { get; set; }
        public string subModel { get; set; }
        public int year { get; set; }
    }

    public class vehicle {
        public Guid vehicleID { get; set; }
        public string vehicleName { get; set; }
        public string MACAddress { get; set; }
    }

    public class vehicleVehicleClass { 
        public Guid vehicleClassID { get; set; }
        public Guid vehicleID { get; set; }
    }

    public class vehicleClass {
        public Guid vehicleClassID { get; set; }
        public string vehicleClassName { get; set; }
    }

    public class vehicleService {
        public Guid vehicleID { get; set; }
        public Guid serviceLookupID { get; set; }
    }

    public class serviceLookup {
        public Guid serviceLookupID { get; set; }
        public string companyName { get; set; }
        public string connString { get; set; }
        public string serviceURL { get; set; }
    }

    public class vehicleClassCAN{
        public Guid VehicleClassCANID { get; set; }
        public Guid vehicleClassID { get; set; }
        public Guid requestID { get; set; }
        public string canRequestGroup { get; set; }
    }

    public class accelVehicleClass {
        public Guid accelValID { get; set; }
        public Guid VehicleClassID { get; set; }
    }

    public class accelVals {
        public Guid accelValID { get; set; }
        public string accelValName { get; set; }
        public int queryRateCAN { get; set; }
        public int sendRateBehave { get; set; }
        public int enableCAN { get; set; }
        public int enableBehave { get; set; }
        public int brakeMilliGs { get; set; }
        public int accelMilliGs { get; set; }
        public int leftTurnMilliGs { get; set; }
        public int rightTurnMilliGs { get; set; }
        public int crashMilliGs { get; set; }
        public int brakeDuration { get; set; }
        public int accelDuration { get; set; }
        public int leftTurnDuration { get; set; }
        public int rightTurnDuration { get; set; }
        public int crashDuration { get; set; }
    }
}