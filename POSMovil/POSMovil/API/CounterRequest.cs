using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace POSMovil.API
{
    public class CounterRequest
    {
        private HttpClient _restclient;
        public CounterRequest(HttpClient restclient)
        {
            if (restclient != null) _restclient = restclient;
            else throw new NullReferenceException("El cliente http no puede se null");
        }

        public async Task<List<Counter>> Get()
        {
            var response = await _restclient.GetAsync<List<Counter>>($"{App.BaseUrl}/factura/counter");

            if (response.Result != null) return response.Result;

            return null;
        }

        public async Task<bool> Update(Counter counter, int id)
        {
            var response = await _restclient.ExecuteAsync<Counter>(Method.POST, $"{App.BaseUrl}/factura/updatecounter/{id}", new Dictionary<string, object>
            {
                { "idfact", counter.idfact },
                { "rlock", counter.rlock }
            });

            return response.Result != null;
        }
    }
}
