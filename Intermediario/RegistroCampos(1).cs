using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intermediario
{
    public class RegistroCampos
    {
        int contador;
        string nombre, apellido, contraseña, cuenta, correo, telefono, direccion;

        public RegistroCampos(string txtnombre, string txtapellido, string txtcontraseña, string txtcuenta, string txtcorreo, string txttelefono, string txtdireccion)
        {
            nombre = txtnombre;
            apellido = txtapellido;
            contraseña = txtcontraseña;
            cuenta = txtcuenta;
            correo = txtcorreo;
            telefono = txttelefono;
            direccion = txtdireccion;
            contador = 0;

        }

        public int CamposLlenos()
        {
            if (nombre != "" || nombre.Length <=30) { contador++; };
            if (apellido != "") { contador++; };
            if (contraseña != "") { contador++; };
            if (cuenta != "") { contador++; };
            if (correo != "") { contador++; };
            if (telefono != "") { contador++; };
            if (direccion != "") { contador++; };
            return contador;

        }
    }
}
