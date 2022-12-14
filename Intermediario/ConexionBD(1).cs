using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;
using Intermediario.MailService;

namespace Intermediario
{
    internal class ConexionBD
    {
        SqlConnection cn = new SqlConnection();

        static string servidor = ".";
        static string bd = "Intermediario";
        /*static string usuario = "joseerivas";
        static string password = "Intermediario32";
        static string puerto = "1433";*/

        string cadenaConexion = "Data source=" + servidor + ";" + "Initial Catalog=" + bd + ";Integrated Security=true";

        public SqlConnection conexion()
        {

            try
            {
                cn.ConnectionString = cadenaConexion;
                cn.Open();
                //MessageBox.Show("Se estableccio la conexion.");
            }
            catch
            {
                MessageBox.Show("Error en la cadena de conexion.");
                Application.Current.Shutdown();
            }


            return cn;
        }

        public SqlConnection CerrarConexion()
        {
            cn.Close();

            return cn;
        }

        public void InsertarBD(string comando)
        {
            ConexionBD dconexion = new ConexionBD();
            dconexion.conexion();

        }

        public string recuperarContraseñaComprador(string solicitudUsuario, string tipoCuenta)
        {
            string consulta = "";
            ConexionBD dConexion = new ConexionBD();

            if(tipoCuenta == "Comprador")
            {
                consulta = "SELECT * FROM [Datos Personales Comprador] WHERE CorreoComprador=@CORREO";
            }
            if(tipoCuenta == "Vendedor")
            {
                consulta = "SELECT * FROM [Datos Personales Vendedor] WHERE CorreoVendedor=@CORREO";
            }

            var cmd = new SqlCommand(consulta,dConexion.conexion());

            cmd.Parameters.AddWithValue("@CORREO", solicitudUsuario);
            var read = cmd.ExecuteReader();

            if(read.Read())
            {
                string nombreUsuario = read.GetString(1) + "," + read.GetString(2);
                string correoUsuario = read.GetString(3);
                string contraseñaUsuario = read.GetString(4);

                ServidorCorreos correos = new ServidorCorreos();
                string subject = "INTERMEDIARIO: Recuperacion de Contraseña";
                string body = "Hola" + nombreUsuario + ".Aqui esta tu contraseña. Recomendamos que borre este e-mail.";
                correos.sendMail(subject,body,correoUsuario);

                MessageBox.Show(nombreUsuario);

                return "Hola" + nombreUsuario + ". Revisa tu correo electronico.";
            }
            else
            {
                return "Lo sentimos. El correo electronico no esta registrado.";
            }


        }
                
    }
}
