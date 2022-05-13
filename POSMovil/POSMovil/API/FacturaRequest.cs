using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace POSMovil.API
{
    public class FacturaRequest
    {
        private HttpClient _restclient;
        public FacturaRequest(HttpClient restclient)
        {
            if (restclient != null) _restclient = restclient;
            else throw new NullReferenceException("El cliente http no puede se null");
        }

        public async Task<List<Factura>> All()
        {
            var response = await _restclient.GetAsync<List<Factura>>($"{App.BaseUrl}/factura/all");

            if (response.Result != null) return response.Result;

            return new List<Factura>();
        }

        public async Task<List<Factura>> Get(int id)
        {

            var response = await _restclient.GetAsync<List<Factura>>($"{App.BaseUrl}/factura/getfact/{id}");

            if (response.Result != null) return response.Result;

            return null;
        }

        public async Task<List<Factura>> GetRI(string user, long auto)
        {

            var response = await _restclient.GetAsync<List<Factura>>($"{App.BaseUrl}/factura/getfactreimp/{user}-{auto}");

            if (response.Result != null) return response.Result;

            return null;
        }

        public async Task<bool> Add(Factura factura)
        {
            var response = await _restclient.ExecuteAsync<Factura>(Method.POST, $"{App.BaseUrl}/factura/addfact", new Dictionary<string, object>
            {
                { "idfact", factura.idfact},
                { "fecha", factura.fecha },
                { "nrofact", factura.nrofact },
                { "nroauto", factura.nroauto },
                { "estado", factura.estado },
                { "nit", factura.nit },
                { "nombre", factura.nombre },
                { "importe", factura.importe.ToString().Replace(',','.') },
                { "ice", factura.ice },
                { "export", factura.export },
                { "vent_tcero", factura.vent_tcero },
                { "subtotal", factura.subtotal },
                { "descuentos", factura.descuentos },
                { "impbase_df", factura.impbase_df },
                { "debitof", factura.debitof },
                { "cod_ctrl", factura.cod_ctrl },
                { "actividad", factura.actividad },
                { "nro_suc", factura.nro_suc },
                { "tot_a_pag", factura.tot_a_pag.ToString().Replace(',','.') },
                { "Recibido", factura.Recibido },
                { "Cambio", factura.Cambio },
                { "hora", factura.hora },
                { "userid", factura.userid },
                { "fechalim", factura.fechalim }
            });

            return response.Result != null;
        }

        public async Task<bool> Update(Factura factura, long id)
        {
            var response = await _restclient.ExecuteAsync<Factura>(Method.POST, $"{App.BaseUrl}/factura/updatefact/{id}", new Dictionary<string, object>
            {
                { "idfact", id },
                { "fecha", factura.fecha },
                { "nrofact", factura.nrofact },
                { "nroauto", factura.nroauto },
                { "estado", factura.estado },
                { "nit", factura.nit },
                { "nombre", factura.nombre },
                { "importe", factura.importe },
                { "ice", factura.ice },
                { "export", factura.export },
                { "vent_tcero", factura.vent_tcero },
                { "subtotal", factura.subtotal },
                { "descuentos", factura.descuentos },
                { "impbase_df", factura.impbase_df },
                { "debitof", factura.debitof },
                { "cod_ctrl", factura.cod_ctrl },
                { "actividad", factura.actividad },
                { "nro_suc", factura.nro_suc },
                { "tot_a_pag", factura.tot_a_pag },
                { "Recibido", factura.Recibido },
                { "Cambio", factura.Cambio },
                { "hora", factura.hora },
                { "userid", factura.userid },
                { "fechalim", factura.fechalim }
            });

            return response.Result != null;
        }

        public async Task<bool> Delete(string id)
        {
            var response = await _restclient.GetAsync<StatusResponse>($"{App.BaseUrl}/factura/deletefact/{id}");

            return response.Result != null && response.Result.code == 200;
        }

    }
}
