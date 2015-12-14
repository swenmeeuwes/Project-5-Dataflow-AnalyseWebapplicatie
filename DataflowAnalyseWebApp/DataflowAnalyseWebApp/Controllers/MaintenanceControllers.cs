using DataflowAnalyseWebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Configuration;
using System.Web.Http;

namespace DataflowAnalyseWebApp.Controllers
{
    public class MaintenanceController : ApiController
    {
        // GET api/values
        public IEnumerable<Position> Get()
        {
            string webserviceUrl = WebConfigurationManager.AppSettings["WebserviceUrl"];
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webserviceUrl + "/positions");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            string responseString = readStream.ReadToEnd();
            PositionResponse positionResponse = JsonConvert.DeserializeObject<PositionResponse>(responseString);

            double d1 = distance(32.9697, -96.80322, 29.46786, -98.53506);

            return positionResponse.result;
        }

        // GET api/values/5
        public string Get(int unitId)
        {
            return "value";
        }

        private double distance(double lat1, double lon1, double lat2, double lon2)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 1.609344;
            return (dist);
        }

        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}
