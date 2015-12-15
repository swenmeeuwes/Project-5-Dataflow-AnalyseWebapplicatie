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
        public IEnumerable<Maintenance> Get()
        {
            return new List<Maintenance>();
        }

        // GET api/maintenance/5
        public Maintenance Get(long unitId)
        {
            string webserviceUrl = WebConfigurationManager.AppSettings["WebserviceUrl"];
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webserviceUrl + "/positions/" + unitId.ToString());
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            string responseString = readStream.ReadToEnd();
            PositionResponse positionResponse = JsonConvert.DeserializeObject<PositionResponse>(responseString);

            Maintenance maintenance = new Maintenance();
            maintenance.unitId = unitId;

            double travelled = 0;
            for (int i = 0; i < positionResponse.result.Length - 1; i++)
            {
                travelled += CalcDistance(positionResponse.result[i].latitudeGps, positionResponse.result[i].longitudeGps, positionResponse.result[i+1].latitudeGps, positionResponse.result[i+1].longitudeGps);
            }
            maintenance.kilometersTravelled = travelled;
            return maintenance;
        }

        private double CalcDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(DegToRad(lat1)) * Math.Sin(DegToRad(lat2)) + Math.Cos(DegToRad(lat1)) * Math.Cos(DegToRad(lat2)) * Math.Cos(DegToRad(theta));
            dist = Math.Acos(dist);
            dist = RadToDeg(dist);
            dist = dist * 1.609344;
            return (dist);
        }

        private double DegToRad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private double RadToDeg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}
