using POSMovil.API;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace POSMovil.Model
{
    public class Client
    {
        public long Nit { get; set; }
        public string Nombre { get; set; }

        public Command<Client> Editar { get; set; }
        public Command<Client> Eliminar { get; set; }

        public static Client FromCliente(Cliente item, Action<Client> editar = null, Action<Client> eliminar = null)
        {
            var client = new Client
            {
                Nit = item.nit,
                Nombre = item.Nombre
            };

            if (editar != null)
            {
                client.Editar = new Command<Client>(editar);
            }
            if (eliminar != null)
            {
                client.Eliminar = new Command<Client>(eliminar);
            }
            return client;
        }
    }
}
