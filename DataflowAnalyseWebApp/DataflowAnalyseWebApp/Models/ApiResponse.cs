using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;

namespace DataflowAnalyseWebApp.Models
{
    [DataContract]
    public class ApiResponse
    {
        [DataMember]
        public int statusCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string errorMessage { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public object result { get; set; }

        public ApiResponse(HttpStatusCode statusCode, object result = null, string errorMessage = null)
        {
            this.statusCode = (int)statusCode;
            this.result = result;
            this.errorMessage = errorMessage;
        }
    }
}