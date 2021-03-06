using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace POSMovil.API
{
    public class HttpResponse<T>
    {
        public T Result { get; set; }
        public StatusResponse Status { get; set; }
        public IRestResponse Response { get; set; }
    }
}
