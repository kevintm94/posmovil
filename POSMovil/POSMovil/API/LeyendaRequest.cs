using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace POSMovil.API
{
    public class LeyendaRequest
    {
        private HttpClient _restclient;
        public LeyendaRequest(HttpClient restclient)
        {
            if (restclient != null) _restclient = restclient;
            else throw new NullReferenceException("El cliente http no puede se null");
        }

        public async Task<List<Leyenda>> All()
        {
            var response = await _restclient.GetAsync<List<Leyenda>>($"{App.BaseUrl}/leyenda");

            if (response.Result != null) return response.Result;

            return new List<Leyenda>();
        }

        public async Task<Leyenda> Get(long id)
        {

            var response = await _restclient.GetAsync<Leyenda>($"{App.BaseUrl}/leyenda/{id}");

            if (response.Result != null) return response.Result;

            return null;
        }

        public async Task<List<Leyenda>> Find(long id)
        {

            var response = await _restclient.GetAsync<List<Leyenda>>($"{App.BaseUrl}/leyenda/find/{id}");

            if (response.Result != null) return response.Result;

            return null;
        }

        public async Task<bool> Add(Leyenda leyenda)
        {
            var response = await _restclient.ExecuteAsync<Producto>(Method.POST, $"{App.BaseUrl}/leyenda", new Dictionary<string, object>
            {
                { "cod_act",     leyenda.cod_act },
                { "descripcio", leyenda.descripcio },
            });

            return response.Result != null;
        }

        public async Task<bool> Update(Leyenda leyenda, long id)
        {
            var response = await _restclient.ExecuteAsync<Cliente>(Method.PUT, $"{App.BaseUrl}/producto/{id}", new Dictionary<string, object>
            {
                { "cod_act",     leyenda.cod_act },
                { "descripcio", leyenda.descripcio }
            });

            return response.Result != null;
        }
    }
}
