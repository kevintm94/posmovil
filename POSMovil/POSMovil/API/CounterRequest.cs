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

        public async Task<Counter> Get(int id)
        {
            var response = await _restclient.GetAsync<Counter>($"{App.BaseUrl}/counter/{id}");

            if (response.Result != null) return response.Result;

            return null;
        }

        public async Task<bool> Update(Counter counter, int id)
        {
            var response = await _restclient.ExecuteAsync<Counter>(Method.PUT, $"{App.BaseUrl}/counter/{id}", new Dictionary<string, object>
            {
                { "idfact", counter.idfact },
                { "nrofact_0", counter.nrofact_0 },
                { "nrofact_1", counter.nrofact_1 },
                { "nrofact_2", counter.nrofact_2 },
                { "rlock", counter.rlock },
                { "a00", counter.a00 },
                { "a01", counter.a01 },
                { "a02", counter.a02 },
                { "a03", counter.a03 }
            });

            return response.Result != null;
        }
    }
}
