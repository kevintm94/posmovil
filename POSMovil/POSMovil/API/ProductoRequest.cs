using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace POSMovil.API
{
    public class ProductoRequest
    {
        private HttpClient _restclient;
        public ProductoRequest(HttpClient restclient)
        {
            if (restclient != null) _restclient = restclient;
            else throw new NullReferenceException("El cliente http no puede se null");
        }

        public async Task<List<Producto>> All()
        {
            var response = await _restclient.GetAsync<List<Producto>>($"{App.BaseUrl}/producto");

            if (response.Result != null) return response.Result;

            return new List<Producto>();
        }

        public async Task<Producto> Get(long id)
        {

            var response = await _restclient.GetAsync<Producto>($"{App.BaseUrl}/producto/{id}");

            if (response.Result != null) return response.Result;

            return null;
        }

        public async Task<List<Producto>> Find(long caeb)
        {

            var response = await _restclient.GetAsync<List<Producto>>($"{App.BaseUrl}/producto/find/{caeb}");

            if (response.Result != null) return response.Result;

            return null;
        }

        public async Task<bool> Add(Producto producto)
        {
            var response = await _restclient.ExecuteAsync<Producto>(Method.POST, $"{App.BaseUrl}/producto", new Dictionary<string, object>
            {
                { "codigo",     producto.codigo },
                { "descripcio", producto.descripcio },
                { "pu",         producto.pu },
                { "codprodsin", producto.codprodsin },
                { "cod_caeb",   producto.cod_caeb },
                { "unidadsin",  producto.unidadsin },
                { "descunisin", producto.descunisin },
            });

            return response.Result != null;
        }

        public async Task<bool> Update(Producto producto, long id)
        {
            var response = await _restclient.ExecuteAsync<Cliente>(Method.PUT, $"{App.BaseUrl}/producto/{id}", new Dictionary<string, object>
            {
                { "codigo",     producto.codigo },
                { "descripcio", producto.descripcio },
                { "pu",         producto.pu },
                { "codprodsin", producto.codprodsin },
                { "cod_caeb",   producto.cod_caeb },
                { "unidadsin",  producto.unidadsin },
                { "descunisin", producto.descunisin },
            });

            return response.Result != null;
        }
    }
}
