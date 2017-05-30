using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace CodeService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "OBDCoder" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select OBDCoder.svc or OBDCoder.svc.cs at the Solution Explorer and start debugging.
    public class OBDCoder : IOBDCoder
    {
        #region " Interfaces "
        public string getCodes(string MACAddress, string control, string type) {
            string ret = string.Empty;
            canObject co;
            switch (type.ToUpper()) {
                case "VIN":
                    /*
                    Guid g;
                    if (!Guid.TryParse(control, out g)) {
                        return "BADGUID";
                    }
                    */
                    co = makeVIN(control);
                    ret = JsonConvert.SerializeObject(co);
                    return ret;
                case "TESTSPEED1":
                    co = makeCAN(type.ToUpper(), MACAddress, control);
                    ret = JsonConvert.SerializeObject(co);
                    return ret;
                default:
                    return "UNKNOWNREQUEST";
            }
        }

        public void setCodeVals(string MACAddress, string control, string data) {
            //for now this is just getting logged to the database
            //ultimately needs to get parsed, processed, and sent to correct vehicle
            SQLCode sql = new SQLCode();
            try {
                sql.logData(data, MACAddress);
            }
            catch (Exception ex) {
                string err = ex.ToString();
            }
        }

        public string getSettings(string MACAddress, string control) {
            try {
                settingObject so = makeSettings(MACAddress);
                string json = JsonConvert.SerializeObject(so);
                return json;
            }
            catch (Exception ex) {
                string err = ex.ToString();
                return err;
            }
        }

        #endregion

        #region " Helpers "

        private canObject makeVIN(string control) {
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

        private canObject makeCAN(string val, string MACAddress, string control) {
            canObject c = new canObject();
            c.can = new List<reqRes>();
            string rString = string.Empty;
            var vcList = from vcc in globalData.vehicleClassCANs
                         join vc in globalData.vehicleClasses on vcc.vehicleClassID equals vc.vehicleClassID
                         join vvc in globalData.vehicleVehicleClasses on vc.vehicleClassID equals vvc.vehicleClassID
                         join v in globalData.vehicles on vvc.vehicleID equals v.vehicleID
                         where vcc.canRequestGroup.ToUpper() == val.ToUpper() && v.MACAddress.ToUpper() == MACAddress.ToUpper()
                         select vcc;
            foreach (vehicleClassCAN vc in vcList) {
                pidRequest req = globalData.pidRequests.Find(delegate (pidRequest find) { return find.requestID == vc.requestID; });
                pidResponse resp = globalData.pidResponses.Find(delegate (pidResponse find) { return find.requestID == vc.requestID; });
                if (req == null || resp == null) {
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

        private settingObject makeSettings(string MACAddress) {
            settingObject so = new settingObject();
            so.getCodeRateSecs = 60;
            so.udpTimeoutSecs = 30;
            so.wifiApnTimeoutSecs = 300;
            so.wifiClientTimeoutSecs = 300;
            so.sftpIPAddress = ConfigurationManager.AppSettings["sftp"].ToString();
            return so;
        }

        #endregion
    }
}
