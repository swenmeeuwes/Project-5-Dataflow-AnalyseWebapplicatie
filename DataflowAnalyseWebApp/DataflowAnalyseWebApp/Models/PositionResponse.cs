﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataflowAnalyseWebApp.Models
{
    public class PositionResponse
    {
        public int statusCode { get; set; }
        public Position[] result { get; set; }
    }
}