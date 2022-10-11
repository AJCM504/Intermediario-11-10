using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sqlcliente;
using System.Windows.Forms;


namespace Intermediario
{
    public class BDConexion
    {
        Sqlconection cn = new Sqlconection();

        static string servidor= "34.135.99.30";
        static string bd="Intermediario";
        static string usuario="sqlserver";
        static string password="Intermediario32";
        static string puerto="1433";

        string cadenaConexion = "Data sourcer" + servidor + "," + puerto + ";user id=" + usuario + ";password=" + password + ";" + "Initial Catalog" + bd + ";Persist Security Info=true";

        public Sqlconnection conexion(){

            try{
                cn.ConnectionString = cadenaConexion;
                cn.Open();
                MessageBox.Show("Se estableccio la conexion.");
            }
            catch(SqlException e)
            {
                MessageBox.Show("No se puedo establecer conexión" +e.toString());
            }


            return cn;
        }
    }
}
