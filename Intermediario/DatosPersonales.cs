using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intermediario
{
    /// <summary>
    /// Clase para almacenar la informacion del usuario.
    /// </summary>
    public class DatosPersonales
    {
        /// <summary>
        /// Nombre del usuario
        /// </summary>
        private static String nombre;
        /// <summary>
        /// Apellido del usuario
        /// </summary>
        private static String apellido;
        /// <summary>
        /// Correo del Usuario
        /// </summary>
        private static String correo;
        /// <summary>
        /// Contraseña del usuario
        /// </summary>
        private static String contraseña;
        /// <summary>
        /// telefono del usuario
        /// </summary>
        private static String telefono;
        /// <summary>
        /// direccion del usuario
        /// </summary>
        private static String direccion;
        /// <summary>
        /// id del usuario
        /// </summary>
        private static String id;
        private static int tipoCuenta;//1 Es comprador, 2 Es Vendedor

        /// <summary>
        /// Confirmador de contraseña
        /// </summary>
        private static String confirmarPass;

        /// <summary>
        /// Obtiene o inserta la confirmacion de contraseña
        /// </summary>
        public String ConfirmarPass { get => confirmarPass; set => confirmarPass = value; }

        /// <summary>
        /// Obtiene o inserta el tipo de cuenta
        /// </summary>
        public int TipoCuenta { get => tipoCuenta; set => tipoCuenta = value; }
        /// <summary>
        /// Obtiene o inserta el id del usuario
        /// </summary>
        public string Id { get => id; set => id = value; }
        /// <summary>
        /// Obtiene o inserta el nombre del usuario
        /// </summary>
        public string Nombre { get => nombre; set => nombre = value; }
        /// <summary>
        /// Obtiene o inserta el apellido del usuario
        /// </summary>
        public string Apellido { get => apellido; set => apellido = value; }
        /// <summary>
        /// Obtiene o inserta el correo del usuario
        /// </summary>
        public string Correo { get => correo; set => correo = value; }
        /// <summary>
        /// Obtiene o inserta la contraseña del usuario
        /// </summary>
        public string Contraseña { get => contraseña; set => contraseña = value; }
        /// <summary>
        /// Obtiene o inserta el telefono del usuario
        /// </summary>
        public string Telefono { get => telefono; set => telefono = value; }
        /// <summary>
        /// Obtiene o inserta la direccion del usuario
        /// </summary>
        public string Direccion { get => direccion; set => direccion = value; }
    }
}