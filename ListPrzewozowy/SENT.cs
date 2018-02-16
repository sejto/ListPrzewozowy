using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ListPrzewozowy
{
   /* public class GoodsSender
    {
        public TraderAddress TraderAddress { get; set; }
        public TraderInfo TraderInfo { get; set; }

    }
    */
    public class ZapiszSENT
    {
        string file = "SENT100.xml";
       
    }

    public class TraderAddress
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string HouseNumber { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
    }
    public class TraderInfo
    {
        public string TraderIdentityNumber { get; set; }
        public string TraderIdentityType { get; set; }
        public string TraderName { get; set; }
    }
}