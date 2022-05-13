using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace POSMovil.API
{
    public class ClientRequest
    {
        private HttpClient _restclient;
        public ClientRequest(HttpClient restclient)
        {
            if (restclient != null) _restclient = restclient;
            else throw new NullReferenceException("El cliente http no puede se null");
        }

        public async Task<List<Cliente>> All() 
        {
            var response = await _restclient.GetAsync<List<Cliente>>($"{App.BaseUrl}/client/all");

            if (response.Result != null) return response.Result;

            return new List<Cliente>();
        }

        public async Task<List<Cliente>> Get(long id)
        {

            var response = await _restclient.GetAsync<List<Cliente>>($"{App.BaseUrl}/client/get/{id}");

            if (response.Result != null) return response.Result;

            return null;
        }

        public async Task<bool> Add(Cliente cliente)
        {
            var response = await _restclient.ExecuteAsync<Cliente>(Method.POST, $"{App.BaseUrl}/client/add", new Dictionary<string, object>
            {
                { "nit", cliente.nit },
                { "Nombre", cliente.Nombre }
            });

            return response.Result != null;
        }

        public async Task<bool> Update(Cliente cliente, long id)
        {
            var response = await _restclient.ExecuteAsync<Cliente>(Method.POST, $"{App.BaseUrl}/client/update/{id}", new Dictionary<string, object>
            {
                { "Nombre", cliente.Nombre }
            });

            return response.Result != null;
        }
    }
}
