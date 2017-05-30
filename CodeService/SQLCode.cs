using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CodeService
{
    public class SQLCode
    {
        private string getConn() {
            return ConfigurationManager.AppSettings["db"];
        }

        #region " log data "

        public void logData(string data, string MACAddress) {
            try {
                using (SqlConnection conn = new SqlConnection(getConn())) {
                    conn.Open();
                    string SQL = "LogCANMsg";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LogID", Guid.NewGuid());
                    cmd.Parameters.AddWithValue("@MACAddress", MACAddress);
                    cmd.Parameters.AddWithValue("@data", data);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.ToString());
            }
        }

        public void logBehavior(string behavior, string MACAddress) {
            try {
                using (SqlConnection conn = new SqlConnection(getConn())) {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("logBehavior", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@behavior", behavior);
                    cmd.Parameters.AddWithValue("@MACAddress", MACAddress);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex) {
                string err = ex.ToString();
            }
        }

        public void logError(string error, string module) {
            try {
                using (SqlConnection conn = new SqlConnection(getConn())) {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("logError", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@error", error);
                    cmd.Parameters.AddWithValue("@module", module);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex) {
                string err = ex.ToString();
            }
        }

        #endregion  

        #region " load data "

        public void loadCANCodeBases() {
            try {
                globalData.canCodeBases.Clear();
                using (SqlConnection conn = new SqlConnection(getConn())) {
                    conn.Open();
                    string SQL = "SELECT * FROM CANCodeBase";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read()) {
                        canCodeBase c = new canCodeBase();
                        c.canCodeBaseID = Guid.Parse(rdr["CANCodeBaseID"].ToString());
                        c.canCodeBaseName = rdr.IsDBNull(1) ? "UNK" : rdr["CANCodeBaseName"].ToString();
                        c.make = rdr.IsDBNull(2) ? "UNK" : rdr["Make"].ToString();
                        c.model = rdr.IsDBNull(3) ? "UNK" : rdr["Model"].ToString();
                        c.subModel = rdr.IsDBNull(4) ? "UNK" : rdr["SubModel"].ToString();
                        c.year = rdr.IsDBNull(5) ? 0 : Convert.ToInt32(rdr["Year"]);
                        globalData.canCodeBases.Add(c);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex) {
                string err = ex.ToString();
                //TODO: add logger
            }
        }

        public void loadCANRequests() {
            try
            {
                globalData.pidRequests.Clear();
                using (SqlConnection conn = new SqlConnection(getConn()))
                {
                    conn.Open();
                    string SQL = "SELECT * FROM CANRequests";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        pidRequest p = new pidRequest();
                        p.requestID = Guid.Parse(rdr["RequestID"].ToString());
                        p.canCodeBaseID = Guid.Parse(rdr["CANCodeBaseID"].ToString());
                        p.requestCode = rdr["RequestCode"].ToString();
                        p.requestName = rdr["RequestName"].ToString();
                        globalData.pidRequests.Add(p);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                //TODO: add logger
            }
        }

        public void loadCANResponses() {
            try
            {
                globalData.pidResponses.Clear();
                using (SqlConnection conn = new SqlConnection(getConn()))
                {
                    conn.Open();
                    string SQL = "SELECT * FROM CANResponses";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        pidResponse p = new pidResponse();
                        p.responseID = Guid.Parse(rdr["ResponseID"].ToString());
                        p.requestID = Guid.Parse(rdr["RequestID"].ToString());
                        p.responseCode = rdr["ResponseCode"].ToString();
                        p.responseType = Convert.ToInt32(rdr["ResponseType"]);
                        globalData.pidResponses.Add(p);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                //TODO: add logger
            }
        }

        public void loadVehicles() {
            try {
                globalData.vehicles.Clear();
                using (SqlConnection conn = new SqlConnection(getConn())) {
                    conn.Open();
                    string SQL = "SELECT * FROM Vehicles";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read()) {
                        vehicle v = new vehicle();
                        v.vehicleID = Guid.Parse(rdr["VehicleID"].ToString());
                        v.vehicleName = rdr.IsDBNull(1) ? "NA" : rdr["VehicleName"].ToString();
                        v.MACAddress = rdr.IsDBNull(2) ? "NA" : rdr["MACAddress"].ToString();
                        globalData.vehicles.Add(v);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
        }

        public void loadVehicleClasses() {
            try {
                globalData.vehicleClasses.Clear();
                using (SqlConnection conn = new SqlConnection(getConn())) {
                    conn.Open();
                    string SQL = "SELECT * FROM VehicleClasses";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read()) {
                        vehicleClass v = new vehicleClass();
                        v.vehicleClassID = Guid.Parse(rdr["VehicleClassID"].ToString());
                        v.vehicleClassName = rdr.IsDBNull(1) ? "NA" : rdr["VehicleClassName"].ToString();
                        globalData.vehicleClasses.Add(v);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex) {
                string err = ex.ToString();
            }
        }

        public void loadVehicleVehicleClasses() {
            try {
                globalData.vehicleVehicleClasses.Clear();
                using (SqlConnection conn = new SqlConnection(getConn())) {
                    conn.Open();
                    string SQL = "SELECT * FROM VehicleVehicleClasses";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read()) {
                        vehicleVehicleClass v = new vehicleVehicleClass();
                        v.vehicleClassID = Guid.Parse(rdr["VehicleClassID"].ToString());
                        v.vehicleID = Guid.Parse(rdr["VehicleID"].ToString());
                        globalData.vehicleVehicleClasses.Add(v);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex) {
                string err = ex.ToString();
            }
        }

        public void loadVehiceServices() {
            try
            {
                globalData.vehicleServices.Clear();
                using (SqlConnection conn = new SqlConnection(getConn()))
                {
                    conn.Open();
                    string SQL = "SELECT * FROM vehicleService";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        vehicleService v = new vehicleService();
                        v.vehicleID = Guid.Parse(rdr["VehicleID"].ToString());
                        v.serviceLookupID = Guid.Parse(rdr["ServiceLookupID"].ToString());
                        globalData.vehicleServices.Add(v);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                //TODO: add logger
            }
        }

        public void loadAccelVehicleClasses() {
            try {
                globalData.accelVehicleClasses.Clear();
                using (SqlConnection conn = new SqlConnection(getConn())) {
                    conn.Open();
                    string SQL = "SELECT * FROM AccelVehicleClass";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read()) {
                        accelVehicleClass a = new accelVehicleClass();
                        a.accelValID = Guid.Parse(rdr["accelValID"].ToString());
                        a.VehicleClassID = Guid.Parse(rdr["VehicleClassID"].ToString());
                        globalData.accelVehicleClasses.Add(a);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex) {
                string err = ex.ToString();
            }
        }

        public void loadAccelVals() {
            try {
                globalData.accelValues.Clear();
                using (SqlConnection conn = new SqlConnection(getConn())) {
                    conn.Open();
                    string SQL = "SELECT * FROM accelVals";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read()) {
                        accelVals a = new accelVals();
                        a.accelValID = Guid.Parse(rdr["accelValID"].ToString());
                        a.accelValName = rdr.IsDBNull(1) ? "NA" : rdr["accelValName"].ToString();
                        a.queryRateCAN = Convert.ToInt32(rdr["queryRateCAN"]);
                        a.sendRateBehave = Convert.ToInt32(rdr["sendRateBehave"]);
                        a.enableCAN = Convert.ToInt32(rdr["enableCAN"]);
                        a.enableBehave = Convert.ToInt32(rdr["enableBehave"]);
                        a.brakeMilliGs = Convert.ToInt32(rdr["brakeMilliGs"]);
                        a.accelMilliGs = Convert.ToInt32(rdr["accelMilliGs"]);
                        a.leftTurnMilliGs = Convert.ToInt32(rdr["leftTurnMilliGs"]);
                        a.rightTurnMilliGs = Convert.ToInt32(rdr["rightTurnMilliGs"]);
                        a.crashMilliGs = Convert.ToInt32(rdr["crashMilliGs"]);
                        a.brakeDuration = Convert.ToInt32(rdr["brakeDuration"]);
                        a.accelDuration = Convert.ToInt32(rdr["accelDuration"]);
                        a.leftTurnDuration = Convert.ToInt32(rdr["leftTurnDuration"]);
                        a.rightTurnDuration = Convert.ToInt32(rdr["rightTurnDuration"]);
                        a.crashDuration = Convert.ToInt32(rdr["crashDuration"]);
                        globalData.accelValues.Add(a);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex) {
                string err = ex.ToString();
            }
        }

        public void loadVehicleClassCANs() {
            try
            {
                globalData.vehicleClassCANs.Clear();
                using (SqlConnection conn = new SqlConnection(getConn()))
                {
                    conn.Open();
                    string SQL = "SELECT * FROM VehicleClassCAN";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        vehicleClassCAN v = new vehicleClassCAN();
                        v.VehicleClassCANID = Guid.Parse(rdr["VehicleClassCANID"].ToString());
                        v.vehicleClassID = Guid.Parse(rdr["VehicleClassID"].ToString());
                        v.requestID = Guid.Parse(rdr["RequestID"].ToString());
                        v.canRequestGroup = rdr["canRequestGroup"].ToString();
                        globalData.vehicleClassCANs.Add(v);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                //TODO: add logger
            }
        }

        public void loadServiceLookups() {
            try
            {
                globalData.serviceLookups.Clear();
                using (SqlConnection conn = new SqlConnection(getConn()))
                {
                    conn.Open();
                    string SQL = "SELECT * FROM ServiceLookup";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        serviceLookup s = new serviceLookup();
                        s.serviceLookupID = Guid.Parse(rdr["ServiceLookupID"].ToString());
                        s.companyName = rdr["CompanyName"].ToString();
                        s.connString = rdr["ConnString"].ToString();
                        s.serviceURL = rdr["ServiceURL"].ToString();
                        globalData.serviceLookups.Add(s);
                    }
                    rdr.Close();
                    rdr = null;
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                //TODO: add logger
            }
        }

        #endregion

        #region " add / update data "

        public void updateCANCodeBase(canCodeBase c) {
            try {
                using (SqlConnection conn = new SqlConnection(getConn())) {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("updateCanCodeBase", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CANCodeBaseID", c.canCodeBaseID);
                    cmd.Parameters.AddWithValue("@CANCodeBaseName", c.canCodeBaseName);
                    cmd.Parameters.AddWithValue("@Make", c.make);
                    cmd.Parameters.AddWithValue("@Model", c.model);
                    cmd.Parameters.AddWithValue("@SubModel", c.subModel);
                    cmd.Parameters.AddWithValue("@Year", c.year);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex) {
                string err = ex.ToString();
                //TODO: Add Logger
            }
        }

        public void deleteCANCodeBase(canCodeBase c) {
            try {
                using (SqlConnection conn = new SqlConnection(getConn())) {
                    conn.Open();
                    //this delete canrequests assigned to this base and then deletes the cancodebase rows
                    string SQL = "deleteCANCodeBase";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CANCodeBaseID", c.canCodeBaseID);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex) {
                string err = ex.ToString();
            }
        }

        public void updateServiceLookup(serviceLookup s)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(getConn()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("updateServiceLookup", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServiceLookupID", s.serviceLookupID);
                    cmd.Parameters.AddWithValue("@CompanyName", s.companyName);
                    cmd.Parameters.AddWithValue("@ConnString", s.connString);
                    cmd.Parameters.AddWithValue("@ServiceURL", s.serviceURL);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                //TODO: Add Logger
            }
        }

        public void deleteServiceLookup(Guid serviceLookupID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(getConn()))
                {
                    conn.Open();
                    string SQL = "deleteServiceLookup";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServiceLookupID", serviceLookupID);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
        }

        public void updateVehicle(vehicle v) {
            try {
                using (SqlConnection conn = new SqlConnection(getConn())) {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("updateVehicle", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VehicleID", v.vehicleID);
                    cmd.Parameters.AddWithValue("@VehicleName", v.vehicleName);
                    cmd.Parameters.AddWithValue("@MACAddress", v.MACAddress);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex) {
                string err = ex.ToString();
                throw new Exception(ex.Message);
            }
        }

        public void deleteVehicle(vehicle v) {
            try
            {
                using (SqlConnection conn = new SqlConnection(getConn()))
                {
                    conn.Open();
                    //remove vehicle from VehicleService, VehicleVehicleClasses, VehicleService, and Vehicles tables
                    string SQL = "deleteVehicle";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VehicleID", v.vehicleID);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
        }

        public void updateVehicleClass(vehicleClass vc) {
            try
            {
                using (SqlConnection conn = new SqlConnection(getConn()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("updateVehicleClass", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VehicleClassID", vc.vehicleClassID);
                    cmd.Parameters.AddWithValue("@VehicleClassName", vc.vehicleClassName);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
        }

        public void deleteVehicleClass(vehicleClass vc) {
            try
            {
                using (SqlConnection conn = new SqlConnection(getConn()))
                {
                    conn.Open();
                    //this sets any vehicles in this class to unassigned, deletes any references in VehicleClassCAN and VehicleClasses
                    string SQL = "deleteVehicleClass";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VehicleClassID", vc.vehicleClassID);
                    cmd.ExecuteNonQuery();
                    cmd = null;

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
        }

        public void updateVehicleClassCAN(List<vehicleClassCAN> vList)
        {
            try
            {
                if (vList.Count == 0)
                {
                    return;
                }
                using (SqlConnection conn = new SqlConnection(getConn()))
                {
                    conn.Open();
                    string SQL = "DELETE VehicleClassCAN WHERE VehicleClassID = '" + vList[0].vehicleClassID + "' AND canRequestGroup = '" + vList[0].canRequestGroup + "'";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    foreach (vehicleClassCAN v in vList)
                    {
                        cmd = new SqlCommand("updateVehicleClassCAN", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@VehicleClassCANID", v.VehicleClassCANID);
                        cmd.Parameters.AddWithValue("@RequestID", v.requestID);
                        cmd.Parameters.AddWithValue("@canRequestGroup", v.canRequestGroup);
                        cmd.Parameters.AddWithValue("@VehicleClassID", v.vehicleClassID);
                        cmd.ExecuteNonQuery();
                        cmd = null;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                //TODO: Add Logger
            }
        }

        public void deleteVehicleClassCANGroup(List<vehicleClassCAN> vList)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(getConn()))
                {
                    conn.Open();
                    string SQL = "deleteVehicleClassCANGroup";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VehicleClassID", vList[0].vehicleClassID);
                    cmd.Parameters.AddWithValue("@canRequestGroup", vList[0].canRequestGroup);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
        }

        public void deleteVehicleClassCAN(vehicleClass v)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(getConn()))
                {
                    conn.Open();
                    string SQL = "deleteVehicleClassCAN";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VehicleClassID", v.vehicleClassID);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
        }

        public void updateVehicleService(vehicleService v) {
            try
            {
                using (SqlConnection conn = new SqlConnection(getConn()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("updateVehicleService", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VehicleID", v.vehicleID);
                    cmd.Parameters.AddWithValue("@ServiceLookupID", v.serviceLookupID);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                //TODO: Add Logger
            }
        }

        public void deleteVehicleService(vehicleService v) {
            try {
                using (SqlConnection conn = new SqlConnection(getConn())) {
                    conn.Open();
                    string SQL = "deleteVehicleService";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VehicleID", v.vehicleID);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex) {
                string err = ex.ToString();
            }
        }

        public void updateVehicleVehicleClass(vehicleVehicleClass v) {
            try {
                using (SqlConnection conn = new SqlConnection(getConn())) {
                    conn.Open();
                    string SQL = "updateVehicleVehicleClass";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VehicleClassID", v.vehicleClassID);
                    cmd.Parameters.AddWithValue("@VehicleID", v.vehicleID);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex) {
                string err = ex.ToString();
            }
        }

        public void deleteVehicleVehicleClass(vehicleVehicleClass v) {
            try {
                using (SqlConnection conn = new SqlConnection(getConn())) {
                    conn.Open();
                    string SQL = "deleteVehicleVehicleClass";
                    SqlCommand cmd = new SqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VehicleID", v.vehicleID);
                    cmd.ExecuteNonQuery();
                    cmd = null;
                    conn.Close();
                }
            }
            catch (Exception ex) {
                string err = ex.ToString();
            }
        }

        #endregion
    }
}