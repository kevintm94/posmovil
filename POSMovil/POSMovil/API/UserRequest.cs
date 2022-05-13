using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace POSMovil.API
{
    public class UserRequest
    {
        private HttpClient _restclient;
        public UserRequest(HttpClient restclient)
        {
            if (restclient != null) _restclient = restclient;
            else throw new NullReferenceException("El cliente http no puede se null");
        }

        public async Task<List<Usuario>> All() 
        {
            
            var response = await _restclient.GetAsync<List<Usuario>>( $"{App.BaseUrl}/user/all");

            if (response.Result != null) return response.Result;

            return new List<Usuario>();
        }

        public async Task<List<Usuario>> Get(string id)
        {

            var response = await _restclient.GetAsync<List<Usuario>>($"{App.BaseUrl}/user/get/{id}");

            if (response.Result != null) return response.Result;

            return null;
        }

        public async Task<bool> Add(Usuario usuario)
        {
            var response = await _restclient.ExecuteAsync<Usuario>(Method.POST, $"{App.BaseUrl}/user/add", new Dictionary<string, object>
            {
                { "id", usuario.id },
                { "nombre", usuario.nombre },
                { "password", usuario.password },
                { "Cmd_99", usuario.Cmd_99 },
                { "Cmd_99_Acc", usuario.Cmd_99_Acc }
            });

            return response.Result != null;
        }

        public async Task<bool> Delete(string id)
        {
            var response = await _restclient.ExecuteAsync<StatusResponse>(Method.GET, $"{App.BaseUrl}/user/delete/{id}");

            return response.Result != null && response.Result.code == 200;
        }

        public async Task<bool> Update(Usuario user, string id)
        {
            var response = await _restclient.ExecuteAsync<Usuario>(Method.POST, $"{App.BaseUrl}/user/update/{id}", new Dictionary<string, object>
            {
                { "id", user.id },
                { "nombre", user.nombre },
                { "password", user.password },
                { "Cmd_99", user.Cmd_99 },
                { "Cmd_99_Acc", user.Cmd_99_Acc }
            });

            return response.Result != null;
        }
    }
}
