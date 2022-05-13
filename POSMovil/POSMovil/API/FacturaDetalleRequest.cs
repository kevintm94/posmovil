using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace POSMovil.API
{
    public class FacturaDetalleRequest
    {
        private HttpClient _restclient;
        public FacturaDetalleRequest(HttpClient restclient)
        {
            if (restclient != null) _restclient = restclient;
            else throw new NullReferenceException("El cliente http no puede se null");
        }

        public async Task<List<FacturaDetalle>> Get(int id)
        {

            var response = await _restclient.GetAsync<List<FacturaDetalle>>($"{App.BaseUrl}/factura/getdetfact/{id}");

            if (response.Result != null) return response.Result;

            return null;
        }

        public async Task<List<FacturaDetalle>> GetRI(int id, long auto)
        {

            var response = await _restclient.GetAsync<List<FacturaDetalle>>($"{App.BaseUrl}/factura/getdetfactreimp/{id}-{auto}");

            if (response.Result != null) return response.Result;

            return null;
        }

        public async Task<bool> Add(FacturaDetalle factura)
        {
            var response = await _restclient.ExecuteAsync<FacturaDetalle>(Method.POST, $"{App.BaseUrl}/factura/adddetfact", new Dictionary<string, object>
            {
                { "idfact", factura.idfact },
                { "nroauto", factura.nroauto },
                { "nrofact", factura.nrofact },
                { "Concepto", factura.Concepto },
                { "subtotal", factura.subtotal.ToString().Replace(',','.') }
            });

            return response.Result != null;
        }

        public async Task<bool> Update(FacturaDetalle factura, long id)
        {
            var response = await _restclient.ExecuteAsync<FacturaDetalle>(Method.POST, $"{App.BaseUrl}/factura/updatedetfact/{id}", new Dictionary<string, object>
            {
                { "idfact", id },
                { "nroauto", factura.nroauto },
                { "nrofact", factura.nrofact },
                { "Concepto", factura.Concepto },
                { "subtotal", factura.subtotal }
            });

            return response.Result != null;
        }

        public async Task<bool> Delete(string id)
        {
            var response = await _restclient.GetAsync<StatusResponse>($"{App.BaseUrl}/factura/deletedetfact/{id}");

            return response.Result != null && response.Result.code == 200;
        }
    }
}
