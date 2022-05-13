using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace POSMovil.API
{
    public class HttpClient
    {

        private Dictionary<string, string> _headers;

        public HttpClient(Dictionary<string, string> headers) 
        {
            if (headers != null) _headers = headers;
            else _headers = new Dictionary<string, string>();
        }

        public string this[string key]
        {
            get
            {
                return _headers[key];
            }
            set
            {
                _headers[key] = value;
            }
        }

        /// <summary>
        /// HTTPGET request
        /// </summary>
        public async Task<HttpResponse<T>> GetAsync<T>(string baseUrl)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(Method.GET);

            //Asignar los headers a la petición
            foreach (var header in _headers)
            {
                request.AddHeader(header.Key, header.Value);
            }

            IRestResponse response = null;
            Exception ex = null;
            try
            {
                response = await client.ExecuteTaskAsync(request);
            }
            catch (Exception _ex)
            {
                ex = _ex;
            }

            var httpresponse = new HttpResponse<T>
            {
                Response = response
            };

            if (response == null)
            {
                httpresponse.Status = new StatusResponse
                {
                    code = -1,
                    message = $"No se pudo obtener una respuesta del servidor, stacktrace: {ex.StackTrace}."
                };
                return httpresponse;
            }

            var jsonresult = response.Content;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var telement = JsonConvert.DeserializeObject<T>(jsonresult);
                httpresponse.Result = telement;
            }
            else if (response.StatusCode == 0)
            {
                httpresponse.Status = new StatusResponse
                {
                    code = 0,
                    message = $"No se pudo obtener una respuesta del servidor."
                };
                return httpresponse;
            } 
            else 
            {
                var status = JsonConvert.DeserializeObject<StatusResponse>(jsonresult);
                httpresponse.Status = status;   
            }

            return httpresponse;
        }


        /// <summary>
        /// HTTPPOST request
        /// </summary>
        public async Task<HttpResponse<T>> PostAsync<T>(string baseUrl, Dictionary<string, object> formdata)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(Method.POST);

            //Asignar los headers a la petición
            foreach (var header in _headers)
            {
                request.AddHeader(header.Key, header.Value);
            }

            foreach (var item in formdata)
            {
                request.AddParameter(item.Key, item.Value);
            }

            IRestResponse response = null;
            Exception ex = null;
            try
            {
                response = await client.ExecuteTaskAsync(request);
            }
            catch (Exception _ex)
            {
                ex = _ex;
            }

            var httpresponse = new HttpResponse<T>
            {
                Response = response
            };

            if (response == null)
            {
                httpresponse.Status = new StatusResponse
                {
                    code = -1,
                    message = $"No se pudo obtener una respuesta del servidor, stacktrace: {ex.StackTrace}."
                };
                return httpresponse;
            }

            var jsonresult = response.Content;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var telement = JsonConvert.DeserializeObject<T>(jsonresult);
                httpresponse.Result = telement;
            }
            else if (response.StatusCode == 0)
            {
                httpresponse.Status = new StatusResponse
                {
                    code = 0,
                    message = $"No se pudo obtener una respuesta del servidor."
                };
                return httpresponse;
            }
            else
            {
                var status = JsonConvert.DeserializeObject<StatusResponse>(jsonresult);
                httpresponse.Status = status;
            }

            return httpresponse;
        }



        /// <summary>
        /// HTTP request
        /// </summary>
        public async Task<HttpResponse<T>> ExecuteAsync<T>(Method method, string baseUrl, Dictionary<string, object> formdata = null)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(method);

            //Asignar los headers a la petición
            foreach (var header in _headers)
            {
                request.AddHeader(header.Key, header.Value);
            }

            if (formdata != null)
            {
                foreach (var item in formdata)
                {
                    request.AddParameter(item.Key, item.Value);
                }
            }

            IRestResponse response = null;
            Exception ex = null;
            try
            {
                response = await client.ExecuteTaskAsync(request);
            }
            catch (Exception _ex)
            {
                ex = _ex;
            }

            var httpresponse = new HttpResponse<T>
            {
                Response = response
            };

            if (response == null)
            {
                httpresponse.Status = new StatusResponse
                {
                    code = -1,
                    message = $"No se pudo obtener una respuesta del servidor, stacktrace: {ex.StackTrace}."
                };
                return httpresponse;
            }

            var jsonresult = response.Content;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var telement = JsonConvert.DeserializeObject<T>(jsonresult);
                httpresponse.Result = telement;
            }
            else if (response.StatusCode == 0)
            {
                httpresponse.Status = new StatusResponse
                {
                    code = 0,
                    message = $"No se pudo obtener una respuesta del servidor."
                };
                return httpresponse;
            }
            else
            {
                var status = JsonConvert.DeserializeObject<StatusResponse>(jsonresult);
                httpresponse.Status = status;
            }

            return httpresponse;
        }

    }
}
