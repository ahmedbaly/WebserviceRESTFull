using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebserviceRESTFull.Models
{
    public class Modelname
    {
        public int model_id { get; set; }
        public string model_name { get; set; }
        public string model_Description { get; set; }
        public byte[] model_photo { get; set; }
        
    }

}
