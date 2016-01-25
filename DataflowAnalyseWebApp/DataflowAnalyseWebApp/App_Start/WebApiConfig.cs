using DataflowAnalyseWebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;

namespace DataflowAnalyseWebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.MessageHandlers.Add(new WrappingHandler());


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
              name: "status",
              routeTemplate: "api/{controller}/{unitId}/{status}",
              defaults: new { unitId = RouteParameter.Optional, status = RouteParameter.Optional }
          );

            config.Routes.MapHttpRoute(
                name: "UnitIdBetweenTimestamps",
                routeTemplate: "api/{controller}/{unitId}/{beginTimestamp}/{endTimestamp}",
                defaults: new { unitId = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                 name: "IntervalDateTime",
                 routeTemplate: "api/{controller}/{beginDate}/{endDate}",
                 defaults: new { beginDate = RouteParameter.Optional, endDate = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "BetweenTimestamps",
                routeTemplate: "api/{controller}/{beginTimestamp}/{endTimestamp}",
                defaults: new { beginTimestamp = RouteParameter.Optional, endTimestamp = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "UnitId",
                routeTemplate: "api/{controller}/{unitId}",
                defaults: new { unitId = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
