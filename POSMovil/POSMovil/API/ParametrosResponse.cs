using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace POSMovil.API
{
    public class ParametrosResponse
    {
        private HttpClient _restclient;
        public ParametrosResponse(HttpClient restclient)
        {
            if (restclient != null) _restclient = restclient;
            else throw new NullReferenceException("El cliente http no puede se null");
        }

        public async Task<List<Parametros>> Get()
        {
            var response = await _restclient.GetAsync<List<Parametros>>($"{App.BaseUrl}/factura/paramgen");

            if (response.Result != null) return response.Result;

            return null;
        }
    }
}
