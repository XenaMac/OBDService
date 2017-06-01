using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Newtonsoft.Json;
using System.Configuration;
using System.ServiceModel.Web;
using System.IO;
using System.Text;

namespace CodeService
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AJAXOBD
    {
        
        #region " Interfaces "
        [OperationContract]
        [WebGet]
        public string getCodes(string MACAddress, string control, string type)
        {
            canObject co = new canObject();
            string ret = string.Empty;
            switch (type.ToUpper())
            {
                case "VIN":
                    /*
                    Guid g;
                    if (!Guid.TryParse(control, out g)) {
                        return "BADGUID";
                    }
                    */
                    co = makeVIN(control);
                    break;
                    //ret = JsonConvert.SerializeObject(co);
                    //return ret;
                case "ALL":
                    co = makeAll(control);
                    break;
                //ret = JsonConvert.SerializeObject(co);
                //return ret;
                default:
                    co = makeCAN(type.ToUpper(), MACAddress, control);
                    break;
            }
            /*
            JsonSerializer js = new JsonSerializer();
            js.TypeNameHandling = TypeNameHandling.Objects;
            MemoryStream ms = new MemoryStream();

            using (StreamWriter sw = new StreamWriter(ms))
            using (JsonWriter jw = new JsonTextWriter(sw)) {
                js.Serialize(jw, co);
                return "";
            }
            */
            return JsonConvert.SerializeObject(co);
        }

        [OperationContract]
        [WebGet]
        public void setCodeVals(string MACAddress, string control, string data)
        {
            //for now this is just getting logged to the database
            //ultimately needs to get parsed, processed, and sent to correct vehicle
            SQLCode sql = new SQLCode();
            try
            {
                sql.logData(data, MACAddress);
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
        }
        [OperationContract]
        [WebGet]
        public string getSettings(string MACAddress, string control)
        {
            try
            {
                settingObject so = makeSettings(MACAddress);
                string json = JsonConvert.SerializeObject(so);
                return json;
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                return err;
            }
        }

        [OperationContract]
        [WebGet]
        public string getConfig(string MACAddress, string control) {
            try {
                accelVals a = makeAccel(MACAddress);
                string ret = "{";
                if (a != null) {
                    ret += "\"queryRateCAN\":" + a.queryRateCAN.ToString() + ",";
                    ret += "\"sendRateBehave\":" + a.sendRateBehave.ToString() + ",";
                    ret += "\"enableCAN\":" + a.enableCAN.ToString() + ",";
                    ret += "\"enableBehave\":" + a.enableBehave.ToString() + ",";
                    ret += "\"brakeMilliGs\":" + a.brakeMilliGs.ToString() + ",";
                    ret += "\"accelMilliGs\":" + a.accelMilliGs.ToString() + ",";
                    ret += "\"leftTurnMilliGs\":" + a.leftTurnMilliGs.ToString() + ",";
                    ret += "\"rightTurnMilliGs\":" + a.rightTurnMilliGs.ToString() + ",";
                    ret += "\"crashMilliGs\":" + a.crashMilliGs.ToString() + ",";
                    ret += "\"brakeDuration\":" + a.brakeDuration.ToString() + ",";
                    ret += "\"accelDuration\":" + a.accelDuration.ToString() + ",";
                    ret += "\"leftTurnDuration\":" + a.leftTurnDuration.ToString() + ",";
                    ret += "\"rightTurnDuration\":" + a.rightTurnDuration.ToString() + ",";
                    ret += "\"crashDuration\":" + a.crashDuration.ToString();
                }
                ret += "}";
                return ret;
            }
            catch (Exception ex) {
                string err = ex.ToString();
                return err;
            }
        }

        [OperationContract]
        [WebGet]
        public string setBehave(string MACAddress, string control, string behavior){
            SQLCode sql = new SQLCode();
            sql.logBehavior(behavior, MACAddress);
            globalData.updateIntelliStuffBehavior(MACAddress, behavior);
            return control;
        }

        #endregion

        #region " Helpers "

        private canObject makeVIN(string control)
        {
            canObject c = new canObject();
            c.can = new List<reqRes>();
            reqRes a = new reqRes();
            a.rr = "7DF#020902|7E8#10,7DF#30|7E8#21,|7E8#22";
            c.can.Add(a);
            //this will ultimately need to come from the database
            /*
            c.can.Add("7DF#020902|7E8#10");
            c.can.Add("7DF#30|7E8#21");
            c.can.Add("|7E8#22");
            */
            c.c = control;
            return c;
        }

        private canObject makeAll(string control) {
            canObject c = new canObject();
            c.can = new List<reqRes>();
            reqRes a = new reqRes();
            a.rr = "7DF#02010C|7E8#04410C";
            reqRes b = new reqRes();
            b.rr = "7DF#02010D|7E8#03410D";
            c.can.Add(a);
            c.can.Add(b);
            c.c = control;
            return c;
        }

        private settingObject makeSettings(string MACAddress)
        {
            settingObject so = new settingObject();
            so.getCodeRateSecs = 60;
            so.udpTimeoutSecs = 30;
            so.wifiApnTimeoutSecs = 300;
            so.wifiClientTimeoutSecs = 300;
            so.sftpIPAddress = ConfigurationManager.AppSettings["sftp"].ToString();
            return so;
        }

        private accelVals makeAccel(string MACAddress) {
            accelVals av = new accelVals();
            var aList = from v in globalData.vehicles
                        join vvc in globalData.vehicleVehicleClasses on v.vehicleID equals vvc.vehicleID
                        join vc in globalData.vehicleClasses on vvc.vehicleClassID equals vc.vehicleClassID
                        join avc in globalData.accelVehicleClasses on vc.vehicleClassID equals avc.VehicleClassID
                        join a in globalData.accelValues on avc.accelValID equals a.accelValID
                        where v.MACAddress == MACAddress
                        select a;
            /*
            var aList = from a in globalData.accelValues
                        join avc in globalData.accelVehicleClasses on a.accelValID equals avc.accelValID
                        join vc in globalData.vehicleClasses on avc.VehicleClassID equals vc.vehicleClassID
                        join vvc in globalData.vehicleVehicleClasses on vc.vehicleClassID equals vvc.vehicleClassID
                        join v in globalData.vehicles on vvc.vehicleID equals v.vehicleID
                        where v.MACAddress == MACAddress
                        select a;
                        */
            foreach (accelVals a in aList) {
                av.accelValID = a.accelValID;
                av.accelValName = a.accelValName;
                av.queryRateCAN = a.queryRateCAN;
                av.sendRateBehave = a.sendRateBehave;
                av.enableCAN = a.enableCAN;
                av.enableBehave = a.enableBehave;
                av.brakeMilliGs = a.brakeMilliGs;
                av.accelMilliGs = a.accelMilliGs;
                av.leftTurnMilliGs = a.leftTurnMilliGs;
                av.rightTurnMilliGs = a.rightTurnMilliGs;
                av.crashMilliGs = a.crashMilliGs;
                av.brakeDuration = a.brakeDuration;
                av.accelDuration = a.accelDuration;
                av.leftTurnDuration = a.leftTurnDuration;
                av.rightTurnDuration = a.rightTurnDuration;
                av.crashDuration = a.crashDuration;
            }
            return av;
        }

        private canObject makeCAN(string val, string MACAddress, string control)
        {
            canObject c = new canObject();
            c.can = new List<reqRes>();
            string rString = string.Empty;
            var vcList = from v in globalData.vehicles
                         join vvc in globalData.vehicleVehicleClasses on v.vehicleID equals vvc.vehicleID
                         join vc in globalData.vehicleClasses on vvc.vehicleClassID equals vc.vehicleClassID
                         join vCAN in globalData.vehicleClassCANs on vc.vehicleClassID equals vCAN.vehicleClassID
                         where v.MACAddress.ToUpper() == MACAddress.ToUpper() && vCAN.canRequestGroup.ToUpper() == val.ToUpper()
                         select vCAN;
            /*
            var vcList = from vcc in globalData.vehicleClassCANs
                         join vc in globalData.vehicleClasses on vcc.vehicleClassID equals vc.vehicleClassID
                         join vccs in globalData.vehicleVehicleClasses on vc.vehicleClassID equals vccs.vehicleClassID
                         join v in globalData.vehicles on vccs.vehicleID equals v.vehicleID
                         where vcc.canRequestGroup.ToUpper() == val.ToUpper() && v.MACAddress.ToUpper() == MACAddress.ToUpper()
                         select vcc;
                         */
            foreach (vehicleClassCAN vc in vcList)
            {
                pidRequest req = globalData.pidRequests.Find(delegate (pidRequest find) { return find.requestID == vc.requestID; });
                pidResponse resp = globalData.pidResponses.Find(delegate (pidResponse find) { return find.requestID == vc.requestID; });
                if (req == null || resp == null)
                {
                    continue;
                }
                rString += req.requestCode + "|" + resp.responseCode + ",";
            }
            rString = rString.Substring(0, rString.Length - 1);
            reqRes a = new reqRes();
            a.rr = rString;
            c.can.Add(a);
            c.c = control;
            return c;
        }

        #endregion
    }
}
