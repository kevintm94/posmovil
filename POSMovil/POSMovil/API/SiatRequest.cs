using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace POSMovil.API
{
    public class SiatRequest
    {
        private HttpClient _restclient;
        public SiatRequest(HttpClient restclient)
        {
            if (restclient != null) _restclient = restclient;
            else throw new NullReferenceException("El cliente http no puede se null");
        }

        public async Task<List<Siat>> All()
        {
            var response = await _restclient.GetAsync<List<Siat>>($"{App.BaseUrl}/siat");

            if (response.Result != null) return response.Result;

            return new List<Siat>();
        }

        public async Task<Siat> Get(long id)
        {

            var response = await _restclient.GetAsync<Siat>($"{App.BaseUrl}/siat/{id}");

            if (response.Result != null) return response.Result;

            return null;
        }
        
        public async Task<Siat> Find(int sucursal, int ptoVenta, string actividad)
        {

            var response = await _restclient.GetAsync<Siat>($"{App.BaseUrl}/siat/{sucursal}/{ptoVenta}/{actividad}");

            if (response.Result != null) return response.Result;

            return null;
        }

        public async Task<bool> Add(Siat siat)
        {
            var response = await _restclient.ExecuteAsync<Siat>(Method.POST, $"{App.BaseUrl}/siat", new Dictionary<string, object>
            {
                { "docsec", siat.docsec },
                { "tipdocsec", siat.tipdocsec },
                { "cod_sis", siat.cod_sis },
                { "ambiente", siat.ambiente },
                { "modalidad", siat.modalidad },
                { "tipo_emi", siat.tipo_emi },
                { "tipo_fact", siat.tipo_fact },
                { "token", siat.token },
                { "token_fv", siat.token_fv },
                { "cuis", siat.cuis },
                { "cuis_fv", siat.cuis_fv },
                { "sinc_cat_f", siat.sinc_cat_f },
                { "cat01", siat.cat01 },
                { "cat02", siat.cat02 },
                { "cat03", siat.cat03 },
                { "cat04", siat.cat04 },
                { "cat05", siat.cat05 },
                { "cat06", siat.cat06 },
                { "cat07", siat.cat07 },
                { "cat08", siat.cat08 },
                { "cat09", siat.cat09 },
                { "cat10", siat.cat10 },
                { "cat11", siat.cat11 },
                { "cat12", siat.cat12 },
                { "cat13", siat.cat13 },
                { "cat14", siat.cat14 },
                { "cat15", siat.cat15 },
                { "cat16", siat.cat16 },
                { "cat17", siat.cat17 },
                { "cufd", siat.cufd },
                { "cufdcdctrl", siat.cufdcdctrl },
                { "cufd_direc", siat.cufd_direc },
                { "cufd_fv", siat.cufd_fv },
                { "cufd_alert", siat.cufd_alert },
                { "cafc", siat.cafc },
                { "cafc2", siat.cafc2 },
                { "formatoimp", siat.formatoimp },
                { "siatserver", siat.siatserver },
                { "ptovta", siat.ptovta },
                { "sucursal", siat.sucursal }
            });

            return response.Result != null;
        }

        public async Task<bool> Update(Siat siat, long id)
        {
            var response = await _restclient.ExecuteAsync<Siat>(Method.PUT, $"{App.BaseUrl}/siat/{id}", new Dictionary<string, object>
            {
                { "docsec", siat.docsec },
                { "tipdocsec", siat.tipdocsec },
                { "cod_sis", siat.cod_sis },
                { "ambiente", siat.ambiente },
                { "modalidad", siat.modalidad },
                { "tipo_emi", siat.tipo_emi },
                { "tipo_fact", siat.tipo_fact },
                { "token", siat.token },
                { "token_fv", siat.token_fv },
                { "cuis", siat.cuis },
                { "cuis_fv", siat.cuis_fv },
                { "sinc_cat_f", siat.sinc_cat_f },
                { "cat01", siat.cat01 },
                { "cat02", siat.cat02 },
                { "cat03", siat.cat03 },
                { "cat04", siat.cat04 },
                { "cat05", siat.cat05 },
                { "cat06", siat.cat06 },
                { "cat07", siat.cat07 },
                { "cat08", siat.cat08 },
                { "cat09", siat.cat09 },
                { "cat10", siat.cat10 },
                { "cat11", siat.cat11 },
                { "cat12", siat.cat12 },
                { "cat13", siat.cat13 },
                { "cat14", siat.cat14 },
                { "cat15", siat.cat15 },
                { "cat16", siat.cat16 },
                { "cat17", siat.cat17 },
                { "cufd", siat.cufd },
                { "cufdcdctrl", siat.cufdcdctrl },
                { "cufd_direc", siat.cufd_direc },
                { "cufd_fv", siat.cufd_fv },
                { "cufd_alert", siat.cufd_alert },
                { "cafc", siat.cafc },
                { "cafc2", siat.cafc2 },
                { "formatoimp", siat.formatoimp },
                { "siatserver", siat.siatserver },
                { "ptovta", siat.ptovta },
                { "sucursal", siat.sucursal }
            });

            return response.Result != null;
        }
    }
}
