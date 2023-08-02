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
            var response = await _restclient.GetAsync<List<Factura>>($"{App.BaseUrl}/cabecera");

            if (response.Result != null) return response.Result;

            return new List<Factura>();
        }

        public async Task<Factura> Get(int id)
        {

            var response = await _restclient.GetAsync<Factura>($"{App.BaseUrl}/cabecera/{id}");

            if (response.Result != null) return response.Result;

            return null;
        }

        public async Task<List<Factura>> GetRI(string user, long auto)
        {

            var response = await _restclient.GetAsync<List<Factura>>($"{App.BaseUrl}/cabecera/getfactreimp/{user}-{auto}");

            if (response.Result != null) return response.Result;

            return null;
        }

        public async Task<bool> Add(Factura factura)
        {
            var response = await _restclient.ExecuteAsync<Factura>(Method.POST, $"{App.BaseUrl}/cabecera", new Dictionary<string, object> 
            {
                { "idfact",     factura.idfact },
                { "fh",         factura.fh.ToString("yyyy-MM-dd HH:mm") },
                { "nrofact",    factura.nrofact },
                { "nit",        factura.nit },
                { "complement", factura.complement },
                { "cdtipodoc",  factura.cdtipodoc },
                { "codclie",    factura.codclie },
                { "nombre",     factura.nombre },
                { "codmetpag",  factura.codmetpag },
                { "nrotarjeta", factura.nrotarjeta },
                { "subtotal",   factura.subtotal },
                { "descuento",  factura.descuento },
                { "total",      factura.total },
                { "gift",       factura.gift },
                { "montoapag",  factura.montoapag },
                { "base_cf",    factura.base_cf },
                { "debitof",    factura.debitof },
                { "fecha_emi",  factura.fecha_emi },
                { "cuf",        factura.cuf },
                { "tipo_fact",  factura.tipo_fact },
                { "tipdocsec",  factura.tipdocsec },
                { "tipo_emi",   factura.tipo_emi },
                { "tipo_emi2",  factura.tipo_emi2 },
                { "codmoneda",  factura.codmoneda },
                { "montotmnd",  factura.montotmnd },
                { "codexcep",   factura.codexcep },
                { "cafc",       factura.cafc },
                { "leyenda",    factura.leyenda },
                { "cod_recep",  factura.cod_recep },
                { "facstatus",  factura.facstatus },
                { "cufd",       factura.cufd },
                { "cufdcdctrl", factura.cufdcdctrl },
                { "pqte",       factura.pqte },
                { "cod_anula",  factura.cod_anula },
                { "cod_ev_sig", factura.cod_ev_sig },
                { "celular_wa", factura.celular_wa },
                { "email",      factura.email },
                { "cod_res",    factura.cod_res },
                { "cod_es",     factura.cod_es },
                { "desc_es",    factura.desc_es },
                { "sucursal",   factura.sucursal },
                { "ptovta",     factura.ptovta },
                { "usercode",   factura.usercode}
            });

            return response.Result != null;
        }

        public async Task<bool> Update(Factura factura, long id)
        {
            var response = await _restclient.ExecuteAsync<Factura>(Method.PUT, $"{App.BaseUrl}/cabecera/{id}", new Dictionary<string, object>
            {
                { "idfact",     id },
                { "fh",         factura.fh },
                { "nrofact",    factura.nrofact },
                { "nit",        factura.nit },
                { "complement", factura.complement },
                { "cdtipodoc",  factura.cdtipodoc },
                { "codclie",    factura.codclie },
                { "nombre",     factura.nombre },
                { "codmetpag",  factura.codmetpag },
                { "nrotarjeta", factura.nrotarjeta },
                { "subtotal",   factura.subtotal },
                { "descuento",  factura.descuento },
                { "total",      factura.total },
                { "gift",       factura.gift },
                { "montoapag",  factura.montoapag },
                { "base_cf",    factura.base_cf },
                { "debitof",    factura.debitof },
                { "fecha_emi",  factura.fecha_emi },
                { "cuf",        factura.cuf },
                { "tipo_fact",  factura.tipo_fact },
                { "tipdocsec",  factura.tipdocsec },
                { "tipo_emi",   factura.tipo_emi },
                { "tipo_emi2",  factura.tipo_emi2 },
                { "codmoneda",  factura.codmoneda },
                { "montotmnd",  factura.montotmnd },
                { "codexcep",   factura.codexcep },
                { "cafc",       factura.cafc },
                { "leyenda",    factura.leyenda },
                { "cod_recep",  factura.cod_recep },
                { "facstatus",  factura.facstatus },
                { "cufd",       factura.cufd },
                { "cufdcdctrl", factura.cufdcdctrl },
                { "pqte",       factura.pqte },
                { "cod_anula",  factura.cod_anula },
                { "cod_ev_sig", factura.cod_ev_sig },
                { "celular_wa", factura.celular_wa },
                { "email",      factura.email },
                { "cod_res",    factura.cod_res },
                { "cod_es",     factura.cod_es },
                { "desc_es",    factura.desc_es },
                { "sucursal",   factura.sucursal },
                { " ptovta",    factura.ptovta }
            });

            return response.Result != null;
        }

        public async Task<bool> Delete(string id)
        {
            var response = await _restclient.ExecuteAsync<StatusResponse>(Method.DELETE, $"{App.BaseUrl}/cabecera/{id}");

            return response.Result != null && response.Result.code == 200;
        }

    }
}
