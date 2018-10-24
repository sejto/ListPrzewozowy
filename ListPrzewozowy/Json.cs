using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ListPrzewozowy
{
    //class Json
    //{
    //    public string response;
    //    public struct coordinates
    //    {
    //        public string latitude { get; set; }
    //        public string longitude { get; set; }
    //        public string city { get; set; }
    //        public string display_name { get; set; }
    //    }
    //    public coordinates Parsuj(string url)
    //    {
    //        using (WebClient webclient = new WebClient())
    //        {
    //            response = webclient.DownloadString(url);
    //        }

    //        dynamic responseArr = JArray.Parse(response);
    //        dynamic RespJson = responseArr[0];

    //        coordinates coordinates = new coordinates();
    //        coordinates.latitude = RespJson.lat;
    //        coordinates.longitude = RespJson.lon;
    //        coordinates.city = RespJson.town;
    //        coordinates.display_name = RespJson.display_name;
    //        return coordinates;
    //    }
    //}

    class Json
    {
        string response;
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string city { get; set; }
        public string display_name { get; set; }

        public Json Parsuj(string url)
        {
            dynamic RespJson;
            using (WebClient webclient = new WebClient())
            {
                response = webclient.DownloadString(url);
            }

            dynamic responseArr = JArray.Parse(response);
            Json coordinates = new Json();
            int c = responseArr.Count;
            if (c>0)
            {
                RespJson = responseArr[0];
                coordinates.latitude = RespJson.lat;
                coordinates.longitude = RespJson.lon;
                coordinates.city = RespJson.town;
                coordinates.display_name = RespJson.display_name;
            }
            else
	{
                MessageBox.Show("Brak takiego adresu w bazie współrzędnych.");
                coordinates.latitude = null;
                coordinates.longitude = null;
                coordinates.city = null;
                coordinates.display_name = null;
            }


            return coordinates;
        }
    }
}