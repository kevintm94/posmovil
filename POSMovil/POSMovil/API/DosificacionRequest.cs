using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace POSMovil.API
{
    public class DosificacionRequest
    {
        private HttpClient _restclient;
        public DosificacionRequest(HttpClient restclient)
        {
            if (restclient != null) _restclient = restclient;
            else throw new NullReferenceException("El cliente http no puede se null");
        }

        public async Task<List<Dosificacion>> Get(long id)
        {
            var response = await _restclient.GetAsync<List<Dosificacion>>($"{App.BaseUrl}/factura/getdosific/{id}");

            if (response.Result != null) return response.Result;

            return null;
        }

        public async Task<bool> Update(Dosificacion dosificacion, long id)
        {
            var response = await _restclient.ExecuteAsync<Dosificacion>(Method.POST, $"{App.BaseUrl}/factura/updatedosific/{id}", new Dictionary<string, object>
            {
                { "ultima", dosificacion.ultima },
                { "rlock", dosificacion.rlock }
            });

            return response.Result != null;
        }
    }
}
