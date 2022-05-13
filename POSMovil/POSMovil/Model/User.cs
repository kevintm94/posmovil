using POSMovil.API;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace POSMovil.Model
{
    public class User
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
        public string Cmd_99 { get; set; }
        public int Cmd_99_Acc { get; set; }

        public Command<User> Editar { get; set; }
        public Command<User> Eliminar { get; set; }

        public static User FromUsuario(Usuario item, Action<User> editar = null, Action<User> eliminar = null) 
        {
            var user = new User
            {
                Nombre = item.nombre,
                Password = item.password,
                Id = item.id,
                Cmd_99 = item.Cmd_99,
                Cmd_99_Acc = item.Cmd_99_Acc
            };

            if (editar != null)
            {
                user.Editar = new Command<User>(editar);
            }
            if (eliminar != null)
            {
                user.Eliminar = new Command<User>(eliminar);
            }
            return user;
        }
    }
}
