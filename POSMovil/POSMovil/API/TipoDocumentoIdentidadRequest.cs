using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;

namespace POSMovil.API
{
    public class TipoDocumentoIdentidadRequest
    {
        private HttpClient _restclient;
        public TipoDocumentoIdentidadRequest(HttpClient restclient)
        {
            if (restclient != null) _restclient = restclient;
            else throw new NullReferenceException("El cliente http no puede se null");
        }

        public async Task<List<TipoDocumentoIdentidad>> All()
        {
            var response = await _restclient.GetAsync<List<TipoDocumentoIdentidad>>($"{App.BaseUrl}/dociden");

            if (response.Result != null) return response.Result;

            return new List<TipoDocumentoIdentidad>();
        }

        public async Task<TipoDocumentoIdentidad> Get(int codigo)
        {

            var response = await _restclient.GetAsync<TipoDocumentoIdentidad>($"{App.BaseUrl}/dociden/{codigo}");

            if (response.Result != null) return response.Result;

            return null;
        }

        public async Task<bool> Add(TipoDocumentoIdentidad dociden)
        {
            var response = await _restclient.ExecuteAsync<TipoDocumentoIdentidad>(Method.POST, $"{App.BaseUrl}/dociden", new Dictionary<string, object>
            {
                { "codigo", dociden.codigo },
                { "descripcio", dociden.descripcio }

            });

            return response.Result != null;
        }

        public async Task<bool> Update(TipoDocumentoIdentidad dociden, int codigo)
        {
            var response = await _restclient.ExecuteAsync<TipoDocumentoIdentidad>(Method.PUT, $"{App.BaseUrl}/dociden/{codigo}", new Dictionary<string, object>
            {
                { "codigo", dociden.codigo },
                { "descripcio", dociden.descripcio }
            });

            return response.Result != null;
        }
    }
}
