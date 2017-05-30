using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

namespace CodeService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IOBDCoder" in both code and config file together.
    [ServiceContract(Namespace = "http://obdcoder.com")]
    public interface IOBDCoder
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "getCodes?MACAddress={MACAddress}&control={control}&type={type}")]
        string getCodes(string MACAddress, string control, string type);

        [OperationContract]
        [WebInvoke(UriTemplate = "setCodeVals?MACAddress={MACAddress}&control={control}&data={data}")]
        void setCodeVals(string MACAddress, string control, string data);

        [OperationContract]
        [WebInvoke(UriTemplate = "getSettings?MACAddress={MACAddress}&control={control}")]
        string getSettings(string MACAddress, string control);
    }
}
