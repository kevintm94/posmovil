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

        public async Task<FacturaDetalle> Get(int id)
        {

            var response = await _restclient.GetAsync<FacturaDetalle>($"{App.BaseUrl}/factura/detalle/{id}");

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
            var response = await _restclient.ExecuteAsync<FacturaDetalle>(Method.POST, $"{App.BaseUrl}/detalle", new Dictionary<string, object>
            {
                { "idfact",     factura.idfact },
                { "codigo",     factura.codigo },
                { "descripcio", factura.descripcio },
                { "pu",         factura.pu.ToString().Replace(',','.') },
                { "cantidad",   factura.cantidad.ToString().Replace(',','.') },
                { "descuento",  factura.descuento.ToString().Replace(',','.') },
                { "subtotal",   factura.subtotal.ToString().Replace(',','.') },
                { "codprodsin", factura.codprodsin },
                { "cod_caeb",   factura.cod_caeb },
                { "unidadsin",  factura.unidadsin },
                { "descunisin", factura.descunisin },
            });

            return response.Result != null;
        }

        public async Task<bool> Update(FacturaDetalle factura, long id)
        {
            var response = await _restclient.ExecuteAsync<FacturaDetalle>(Method.PUT, $"{App.BaseUrl}/detalle/{id}", new Dictionary<string, object>
            {
                { "idfact",     factura.idfact },
                { "codigo",     factura.codigo },
                { "descripcio", factura.descripcio },
                { "pu",         factura.pu.ToString().Replace(',','.') },
                { "cantidad",   factura.cantidad },
                { "descuento",  factura.descuento.ToString().Replace(',','.') },
                { "subtotal",   factura.subtotal.ToString().Replace(',','.') },
                { "codprodsin", factura.codprodsin },
                { "cod_caeb",   factura.cod_caeb },
                { "unidadsin",  factura.unidadsin },
                { "descunisin", factura.descunisin }
            });

            return response.Result != null;
        }

        public async Task<bool> Delete(string id)
        {
            var response = await _restclient.ExecuteAsync<StatusResponse>(Method.DELETE, $"{App.BaseUrl}/detalle/{id}");

            return response.Result != null && response.Result.code == 200;
        }
    }
}
