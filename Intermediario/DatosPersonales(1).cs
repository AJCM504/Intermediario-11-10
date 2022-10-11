using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intermediario
{
    public class DatosPersonales
    {
        private static String nombre;
        private static String apellido;
        private static String correo;
        private static String contraseña;
        private static String telefono;
        private static String direccion;
        private static String id;
        private static int tipoCuenta;//1 Es comprador, 2 Es Vendedor

        public int TipoCuenta { get => tipoCuenta; set => tipoCuenta = value; }
        public string Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public string Correo { get => correo; set => correo = value; }
        public string Contraseña { get => contraseña; set => contraseña = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Direccion { get => direccion; set => direccion = value; }
    }
}