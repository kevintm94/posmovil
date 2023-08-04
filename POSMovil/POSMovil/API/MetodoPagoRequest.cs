using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace POSMovil.API
{
    public class MetodoPagoRequest
    {
        private HttpClient _restclient;
        public MetodoPagoRequest(HttpClient restclient)
        {
            if (restclient != null) _restclient = restclient;
            else throw new NullReferenceException("El cliente http no puede se null");
        }

        public async Task<List<MetodoPago>> All()
        {
            var response = await _restclient.GetAsync<List<MetodoPago>>($"{App.BaseUrl}/mpago");

            if (response.Result != null) return response.Result;

            return new List<MetodoPago>();
        }

        public async Task<MetodoPago> Get(int codigo)
        {

            var response = await _restclient.GetAsync<MetodoPago>($"{App.BaseUrl}/mpago/{codigo}");

            if (response.Result != null) return response.Result;

            return null;
        }

    }
}
