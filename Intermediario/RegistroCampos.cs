using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;

namespace Intermediario
{
    /// <summary>
    /// Clase para verificar que los campos no esten vacios
    /// </summary>
    public class RegistroCampos
    {
        int contador;
        //string nombre, apellido, contraseña, cuenta, correo, telefono, direccion;

        List<string> campos = new List<string>();

        /// <summary>
        /// Metodo contructor de la clase RegistroCampos.
        /// Obtiene e ingresa los datos.
        /// </summary>
        /// <param name="txtnombre">Nombre</param>
        /// <param name="txtapellido">Apellido</param>
        /// <param name="txtcontraseña">Contraseña</param>
        /// <param name="txtcuenta">Tipo de cuenta</param>
        /// <param name="txtcorreo">Correo</param>
        /// <param name="txttelefono">Telefono</param>
        /// <param name="txtdireccion">Direccion</param>
        public RegistroCampos(string txtnombre, string txtapellido, string txtcontraseña, string txtcuenta, string txtcorreo, string txttelefono, string txtdireccion)
        {

            campos.Add(txtnombre);
            campos.Add(txtapellido);
            campos.Add(txtcontraseña);
            campos.Add(txtcuenta);
            campos.Add(txtcorreo);
            campos.Add(txttelefono);
            campos.Add(txtdireccion);

        }

        /// <summary>
        /// Metodo contructor de la clase RegistroCampos.
        /// Obtiene e ingresa los datos.
        /// </summary>
        /// <param name="nombreProducto">Nombre del Producto</param>
        /// <param name="categoria">Categoria</param>
        /// <param name="precio">Precio</param>
        /// <param name="marca">Marca</param>
        /// <param name="modelo">Modelo</param>
        /// <param name="cantidad">Cantidad</param>
        /// <param name="descripcion">Descripcion</param>
        /// <param name="foto">Foto</param>
        public RegistroCampos(string nombreProducto,string categoria, string precio, string marca, string modelo, string cantidad, string descripcion, string foto)
        {
            campos.Add(nombreProducto);
            campos.Add(categoria);
            campos.Add(precio);
            campos.Add(marca);
            campos.Add(modelo);
            campos.Add(cantidad);
            campos.Add(descripcion);
            campos.Add(foto);
        }

        /// <summary>
        /// Verifica que ninguno de los datos obtenidos en el metodo contrusctor este vacio
        /// </summary>
        /// <returns></returns>
        public int CamposLlenos()
        {
            /*if (nombre != "" || nombre.Length <=30) { contador++; };
            if (apellido != "") { contador++; };
            if (contraseña != "") { contador++; };
            if (cuenta != "") { contador++; };
            if (correo != "") { contador++; };
            if (telefono != "") { contador++; };
            if (direccion != "") { contador++; };*/

            for (int i = 0; i < campos.Count; i++)
            {
                if (campos[i] != "" && campos[i] != "0") { contador++; }
            }
            //MessageBox.Show(Convert.ToString(contador));

            return contador;

        }
        /// <summary>
        /// Verifica que la contraseña cumpla con al menos una letra, un numero y un caracter especial.
        /// </summary>
        /// <param name="contraseñaSinVerificar">Contraseña a Verificar</param>
        /// <returns>Retorna true de ser segura o false de ser insegura</returns>
        public Boolean ContrasenaSegura(String contraseñaSinVerificar)
        {
            //letras de la A a la Z, mayusculas y minusculas
            Regex letras = new Regex(@"[a-zA-z]");
            //digitos del 0 al 9
            Regex numeros = new Regex(@"[0-9]");
            //cualquier caracter del conjunto
            Regex caracEsp = new Regex("[!\"#\\$%&'()*+,-./:;=?@*\\[\\]^_`{|}~]");

            Boolean cumpleCriterios = false;

            //si no contiene las letras, regresa false
            if (!letras.IsMatch(contraseñaSinVerificar))
            {
                return false;
            }
            //si no contiene los numeros, regresa false
            if (!numeros.IsMatch(contraseñaSinVerificar))
            {
                return false;
            }

            //si no contiene los caracteres especiales, regresa false
            if (!caracEsp.IsMatch(contraseñaSinVerificar))
            {
                return false;
            }

            //si cumple con todo, regresa true
            return true;
        }

        /// <summary>
        /// Verifica que el numero de telefono sea de una compañia hondureña
        /// </summary>
        /// <param name="numero">Numero de telefono a verificar</param>
        /// <returns>true de ser Numero de telefono hondureño y false de no serlo</returns>
        public Boolean VerificarNumeroMovil(String numero)
        {
            
            string codigoMovil = numero.Substring(0,1);
            

            if (codigoMovil != "9" && codigoMovil != "8" && codigoMovil != "7" && codigoMovil != "3")
            {
               
                return false;
            }

            return true;
        }
    }
}
